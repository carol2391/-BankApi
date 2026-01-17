using System.Threading.Tasks;
using BankApi.Domain.Entities;

namespace BankApi.Application.Interfaces
{
    public interface ICuentaRepository
    {
        Task<Cuenta?> ObtenerCuentaPorNumeroAsync(string numeroCuenta);

        Task GuardarCambiosAsync();

        Task<Cuenta> CrearCuentaAsync(Cuenta cuenta);

        Task<Cuenta?> ObtenerPorNumeroAsync(string numeroCuenta);
    }
}
