using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApi.Domain.Entities
{
    public class Cuenta
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string NumeroCuenta { get; set; } = string.Empty;

        [Required]
        public decimal Saldo { get; set; }

        [Required]
        public int ClienteId { get; set; }

        public Cliente Cliente { get; set; } = null!;
        public List<Transaccion> Transacciones { get; set; } = new();
    }
}
