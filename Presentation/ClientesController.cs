using BankApi.Application.Services;
using BankApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Presentation.Controllers;

[ApiController]
[Route("api/clientes")]
public class ClientesController : ControllerBase
{
    private readonly ClienteService _clienteService;

    public ClientesController(ClienteService clienteService)
    {
        _clienteService = clienteService;
    }

    [HttpPost]
    public async Task<ActionResult<Cliente>> CrearCliente([FromBody] Cliente cliente)
    {
        var creado = await _clienteService.CrearClienteAsync(cliente);

        return CreatedAtAction(nameof(ObtenerCliente), new { id = creado.Id }, creado);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cliente>> ObtenerCliente(int id)
    {
        var cliente = await _clienteService.ObtenerClientePorIdAsync(id);
        if (cliente == null)
            return NotFound();
        return Ok(cliente);
    }
}
