using System;
using System.Threading.Tasks;
using BankApi.Domain.Entities;

namespace BankApi.Application.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente> CrearClienteAsync(Cliente cliente);
        Task<Cliente?> ObtenerClientePorIdAsync(int id);
    }
}
