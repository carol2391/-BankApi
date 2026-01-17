using System;
using System.Threading.Tasks;
using BankApi.Application.Interfaces;
using BankApi.Domain.Entities;

namespace BankApi.Application.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepo;

        public ClienteService(IClienteRepository clienteRepo)
        {
            _clienteRepo = clienteRepo;
        }

        public async Task<Cliente> CrearClienteAsync(Cliente cliente)
        {
            return await _clienteRepo.CrearClienteAsync(cliente);
        }

        public async Task<Cliente?> ObtenerClientePorIdAsync(int id)
        {
            return await _clienteRepo.ObtenerClientePorIdAsync(id);
        }
    }
}
