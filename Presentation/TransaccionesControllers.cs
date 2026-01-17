using BankApi.Application.Dto;
using BankApi.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace BankApi.Presentation.Controllers
{
    [ApiController]
    [Route("api/transacciones")]
    public class TransaccionesController : ControllerBase
    {
        private readonly TransaccionService _transaccionService;

        public TransaccionesController(TransaccionService service) => _transaccionService = service;

        [HttpPost("deposito")]
        public async Task<IActionResult> Depositar([FromBody] TransaccionRequest request)
        {
            try
            {
                await _transaccionService.RealizarDepositoAsync(
                    request.NumeroCuenta,
                    request.Monto
                );
                return Ok(new { mensaje = "Depósito exitoso" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("retiro")]
        public async Task<IActionResult> Retirar([FromBody] TransaccionRequest request)
        {
            try
            {
                await _transaccionService.RealizarRetiroAsync(request.NumeroCuenta, request.Monto);
                return Ok(new { mensaje = "Retiro exitoso" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{numeroCuenta}/resumen")]
        public async Task<IActionResult> GetResumen(string numeroCuenta)
        {
            var resumen = await _transaccionService.ObtenerResumenAsync(numeroCuenta);
            return Ok(resumen);
        }
    }
}
