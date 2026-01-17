using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApi.Application.Dto
{
    public class CrearCuentaRequest
    {
        public string NumeroCuenta { get; set; } = null!;
        public decimal SaldoInicial { get; set; }
        public int ClienteId { get; set; }
    }

    public class CrearCuentaResponse
    {
        public int Id { get; set; }

        [Required]
        public string NumeroCuenta { get; set; }

        [Required]
        public decimal Saldo { get; set; }

        [Required]
        public int ClienteId { get; set; }
    }

    public class TransaccionResponse
    {
        public decimal Monto { get; set; }
        public string Tipo { get; set; } = null!;
        public decimal SaldoDespues { get; set; }
        public DateTime Fecha { get; set; }
    }

    public class TransaccionRequest
    {
        public decimal Monto { get; set; }
        public string NumeroCuenta { get; set; }
    }

    public class ResumenTransaccionesResponse
    {
        public string NumeroCuenta { get; set; } = string.Empty;
        public decimal SaldoFinalCalculado { get; set; }
        public List<TransaccionResponse> Movimientos { get; set; } = new();
    }
}
