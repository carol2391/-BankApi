using BankApi.Application.Dto;
using BankApi.Application.Interfaces;
using BankApi.Domain.Entities;

namespace BankApi.Application.Services
{
    public class TransaccionService
    {
        private readonly ITransaccionRepository _transaccionRepo;
        private readonly ICuentaRepository _cuentaRepo;

        public TransaccionService(ITransaccionRepository tRepo, ICuentaRepository cRepo)
        {
            _transaccionRepo = tRepo;
            _cuentaRepo = cRepo;
        }

        public async Task RealizarDepositoAsync(string numeroCuenta, decimal monto)
        {
            var cuenta =
                await _cuentaRepo.ObtenerPorNumeroAsync(numeroCuenta)
                ?? throw new KeyNotFoundException("La cuenta no existe");

            cuenta.Saldo += monto;

            var transaccion = new Transaccion
            {
                CuentaId = cuenta.Id,
                Monto = monto,
                Tipo = TipoTransaccion.Deposito,
                SaldoDespues = cuenta.Saldo,
                Fecha = DateTime.UtcNow,
            };

            await _transaccionRepo.RegistrarAsync(transaccion);

            await _cuentaRepo.GuardarCambiosAsync();
        }

        public async Task RealizarRetiroAsync(string numeroCuenta, decimal monto)
        {
            var cuenta =
                await _cuentaRepo.ObtenerPorNumeroAsync(numeroCuenta)
                ?? throw new KeyNotFoundException("La cuenta no existe");

            if (cuenta.Saldo < monto)
            {
                throw new InvalidOperationException("Saldo insuficiente para realizar el retiro.");
            }

            cuenta.Saldo -= monto;

            var transaccion = new Transaccion
            {
                CuentaId = cuenta.Id,
                Monto = monto,
                Tipo = TipoTransaccion.Retiro,
                SaldoDespues = cuenta.Saldo,
                Fecha = DateTime.UtcNow,
            };

            await _transaccionRepo.RegistrarAsync(transaccion);
            await _cuentaRepo.GuardarCambiosAsync();
        }

        public async Task<ResumenTransaccionesResponse> ObtenerResumenAsync(string numeroCuenta)
        {
            var cuenta =
                await _cuentaRepo.ObtenerPorNumeroAsync(numeroCuenta)
                ?? throw new KeyNotFoundException("Cuenta no encontrada");

            var transacciones = await _transaccionRepo.ObtenerTodasPorCuentaAsync(
                cuenta.NumeroCuenta
            );

            var movimientos = transacciones
                .Select(t => new TransaccionResponse
                {
                    Monto = t.Monto,
                    Tipo = t.Tipo.ToString(),
                    SaldoDespues = t.SaldoDespues,
                    Fecha = t.Fecha,
                })
                .ToList();

            return new ResumenTransaccionesResponse
            {
                NumeroCuenta = numeroCuenta,
                SaldoFinalCalculado = cuenta.Saldo,
                Movimientos = movimientos,
            };
        }

        // DENTRO DE TransaccionService.cs
        public async Task AplicarInteresesAsync(string numeroCuenta, decimal tasa)
        {
            // 1. Buscamos la CUENTA (porque ahí está el saldo)
            var cuenta =
                await _cuentaRepo.ObtenerPorNumeroAsync(numeroCuenta)
                ?? throw new KeyNotFoundException();

            // 2. Calculamos el monto (Lógica de negocio)
            decimal montoInteres = cuenta.Saldo * tasa;

            // 3. Afectamos la CUENTA
            cuenta.Saldo += montoInteres;

            // 4. Creamos la TRANSACCIÓN para el historial
            var transaccion = new Transaccion
            {
                CuentaId = cuenta.Id,
                Monto = montoInteres,
                Tipo = TipoTransaccion.Deposito, // El interés suma, es un depósito
                SaldoDespues = cuenta.Saldo,
                Fecha = DateTime.UtcNow,
                // Opcional: una nota que diga "Intereses mensuales"
            };

            // 5. Persistencia
            await _transaccionRepo.RegistrarAsync(transaccion);
            await _cuentaRepo.GuardarCambiosAsync();
        }
    }
}
