using System;
using System.Threading.Tasks;
using BankApi.Application.Interfaces;
using BankApi.Domain.Entities;
using BankApi.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly BankDbContext _context;

        public ClienteRepository(BankDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente> CrearClienteAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente?> ObtenerClientePorIdAsync(int id)
        {
            return await _context
                .Clientes.Include(c => c.Cuentas)
                .FirstOrDefaultAsync(c => c.Id == id);
        }
    }
}
