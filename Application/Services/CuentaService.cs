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
        private readonly IClienteRepository _clienteRepo;

        public CuentaService(
            ICuentaRepository cuentaRepo,
            ITransaccionRepository transaccionRepo,
            IClienteRepository clienteRepo
        )
        {
            _cuentaRepo = cuentaRepo;
            _transaccionRepo = transaccionRepo;
            _clienteRepo = clienteRepo;
        }

        public async Task<CrearCuentaResponse> CrearCuentaAsync(CrearCuentaRequest dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Error al crear la cuenta");

            var clienteExiste = await _clienteRepo.ObtenerClientePorIdAsync(dto.ClienteId);

            if (clienteExiste == null)
                throw new KeyNotFoundException($"No existe el cliente con id {dto.ClienteId}");
            if (dto.SaldoInicial < 0)
                throw new ArgumentException(
                    "El saldo inicial no puede ser negativo",
                    nameof(dto.SaldoInicial)
                );

            var nuevaCuenta = new Cuenta
            {
                NumeroCuenta = Guid.NewGuid().ToString().Substring(0, 10),
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
            if (string.IsNullOrWhiteSpace(numeroCuenta))
                throw new ArgumentException(
                    "El número de cuenta es obligatorio",
                    nameof(numeroCuenta)
                );
            var cuenta =
                await _cuentaRepo.ObtenerCuentaPorNumeroAsync(numeroCuenta)
                ?? throw new KeyNotFoundException("Cuenta no encontrada");

            return cuenta.Saldo;
        }
    }
}
