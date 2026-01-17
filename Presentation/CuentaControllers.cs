
using BankApi.Application.Dto;
using BankApi.Application.Services;
using BankApi.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly CuentaService _serviceCuenta;

        public CuentasController(CuentaService service) => _serviceCuenta = service;

        public async Task<CrearCuentaResponse> CrearCuentaAsync(CrearCuentaRequest dto)
        {
            var cuentaCreada = await _serviceCuenta.CrearCuentaAsync(dto);
            return cuentaCreada;
        }

        [HttpGet("{numeroCuenta}/saldo")]
        public async Task<IActionResult> ConsultarSaldo(string numeroCuenta)
        {
            var saldo = await _serviceCuenta.ConsultarSaldoAsync(numeroCuenta);
            return Ok(saldo);
        }
    }
}
