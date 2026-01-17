using System.Collections.Generic;
using System.Threading.Tasks;
using BankApi.Application.Dto;
using BankApi.Application.Interfaces;
using BankApi.Application.Services;
using BankApi.Domain.Entities;

namespace BankApi.Application.Services
{
    public class CuentaService
    {
        private readonly ICuentaRepository _cuentaRepo;
        private readonly ITransaccionRepository _transaccionRepo;

        public CuentaService(ICuentaRepository cuentaRepo, ITransaccionRepository transaccionRepo)
        {
            _cuentaRepo = cuentaRepo;
            _transaccionRepo = transaccionRepo;
        }

        public async Task<CrearCuentaResponse> CrearCuentaAsync(CrearCuentaRequest dto)
        {
            var nuevaCuenta = new Cuenta
            {
                NumeroCuenta = Guid.NewGuid().ToString().Substring(0, 10), // Número más corto
                Saldo = dto.SaldoInicial,
                ClienteId = dto.ClienteId,
            };

            await _cuentaRepo.CrearCuentaAsync(nuevaCuenta);
            await _cuentaRepo.GuardarCambiosAsync();

            if (dto.SaldoInicial > 0)
            {
                var depositoInicial = new Transaccion
                {
                    CuentaId = nuevaCuenta.Id,
                    Tipo = TipoTransaccion.Deposito,
                    Monto = dto.SaldoInicial,
                    SaldoDespues = nuevaCuenta.Saldo,
                    Fecha = DateTime.UtcNow,
                };
                await _transaccionRepo.RegistrarAsync(depositoInicial);
            }

            return new CrearCuentaResponse
            {
                Id = nuevaCuenta.Id,
                NumeroCuenta = nuevaCuenta.NumeroCuenta,
                Saldo = nuevaCuenta.Saldo,
                ClienteId = nuevaCuenta.ClienteId,
            };
        }

        public async Task<decimal> ConsultarSaldoAsync(string numeroCuenta)
        {
            var cuenta =
                await _cuentaRepo.ObtenerCuentaPorNumeroAsync(numeroCuenta)
                ?? throw new KeyNotFoundException("Cuenta no encontrada");

            return cuenta.Saldo;
        }
    }
}
