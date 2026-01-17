using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApi.Domain.Entities
{
    public enum TipoTransaccion
    {
        Deposito,
        Retiro,
    }

    public class Transaccion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public TipoTransaccion Tipo { get; set; }

        [Required]
        public decimal Monto { get; set; }

        [Required]
        public decimal SaldoDespues { get; set; }

        [Required]
        public DateTime Fecha { get; set; } = DateTime.UtcNow;

        [Required]
        public int CuentaId { get; set; }

        [ForeignKey("CuentaId")]
        [Required]
        public Cuenta? Cuenta { get; set; }
    }

    public class TransaccionDTO
    {
        public decimal Monto { get; set; }
        public decimal SaldoDespues { get; set; }
        public TipoTransaccion Tipo { get; set; }
        public DateTime Fecha { get; set; }
    }
}
