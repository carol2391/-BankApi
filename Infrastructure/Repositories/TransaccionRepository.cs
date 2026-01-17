using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BankApi.Application.Interfaces;
using BankApi.Domain.Entities;
using BankApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Infrastructure.Repositories
{
    public class TransaccionRepository : ITransaccionRepository
    {
        private readonly BankDbContext _context;

        public TransaccionRepository(BankDbContext context)
        {
            _context = context;
        }

        public async Task RegistrarAsync(Transaccion transaccion)
        {
            await _context.Transacciones.AddAsync(transaccion);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Transaccion>> ObtenerPorCuentaIdAsync(int cuentaId)
        {
            return await _context
                .Transacciones.Where(t => t.CuentaId == cuentaId)
                .OrderByDescending(t => t.Fecha)
                .ToListAsync();
        }

        public async Task<List<Transaccion>> ObtenerTodasPorCuentaAsync(string numeroCuenta)
        {
            return await _context
                .Transacciones.Where(t => t.Cuenta.NumeroCuenta == numeroCuenta) 
                .OrderByDescending(t => t.Fecha)
                .ToListAsync();
        }

        public async Task GuardarCambiosAsync() => await _context.SaveChangesAsync();
    }
}
