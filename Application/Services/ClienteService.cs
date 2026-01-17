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
            if (cliente == null)
                throw new ArgumentNullException("Error al crear el cliente");

            try
            {
                return await _clienteRepo.CrearClienteAsync(cliente);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Error al crear el cliente", ex);
            }
        }

        public async Task<Cliente?> ObtenerClientePorIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El id debe ser mayor que cero", nameof(id));

            var cliente = await _clienteRepo.ObtenerClientePorIdAsync(id);

            if (cliente == null)
                throw new KeyNotFoundException($"No se encontró el cliente con id {id}");

            return cliente;
        }
    }
}
