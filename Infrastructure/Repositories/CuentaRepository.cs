using BankApi.Application.Interfaces;
using BankApi.Domain.Entities;
using BankApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Infrastructure.Repositories
{
    public class CuentaRepository : ICuentaRepository
    {
        private readonly BankDbContext _context;

        public CuentaRepository(BankDbContext context) => _context = context;

        public async Task<Cuenta?> ObtenerCuentaPorNumeroAsync(string numeroCuenta)
        {
            return await _context.Cuentas.FirstOrDefaultAsync(c => c.NumeroCuenta == numeroCuenta);
        }

        public async Task<Cuenta?> ObtenerPorNumeroAsync(string numeroCuenta) =>
            await ObtenerCuentaPorNumeroAsync(numeroCuenta);

        public async Task<Cuenta> CrearCuentaAsync(Cuenta cuenta)
        {
            await _context.Cuentas.AddAsync(cuenta);
            return cuenta;
        }

        public async Task GuardarCambiosAsync() => await _context.SaveChangesAsync();
    }
}
