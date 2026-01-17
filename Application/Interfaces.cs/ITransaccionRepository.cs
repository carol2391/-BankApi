using BankApi.Domain.Entities;

namespace BankApi.Application.Interfaces
{
    public interface ITransaccionRepository
    {
        Task RegistrarAsync(Transaccion transaccion);
        Task<List<Transaccion>> ObtenerTodasPorCuentaAsync(string numeroCuenta);
        Task GuardarCambiosAsync();
    }
}
