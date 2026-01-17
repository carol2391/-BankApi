using BankApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BankApi.Infrastructure.Database
{
    public class BankDbContext : DbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Cliente>().Property(c => c.Id).ValueGeneratedOnAdd();


            modelBuilder.Entity<Cuenta>().Property(c => c.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Transaccion>().Property(t => t.Id).ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Cliente> Clientes => Set<Cliente>();
        public DbSet<Cuenta> Cuentas => Set<Cuenta>();
        public DbSet<Transaccion> Transacciones => Set<Transaccion>();
    }
}
