using BankApi.Application.Interfaces;
using BankApi.Application.Services;
using BankApi.Domain.Entities;
using FluentAssertions;
using Moq;
using Xunit;

namespace BankApi.Tests
{
    public class BancoLogicaTests
    {
        private readonly Mock<ICuentaRepository> _cuentaRepoMock;
        private readonly Mock<ITransaccionRepository> _transaccionRepoMock;
        private readonly Mock<IClienteRepository> _clienteRepoMock;
        private readonly TransaccionService _transaccionService;
        private readonly ClienteService _clienteService;

        public BancoLogicaTests()
        {
            _cuentaRepoMock = new Mock<ICuentaRepository>();
            _transaccionRepoMock = new Mock<ITransaccionRepository>();
            _clienteRepoMock = new Mock<IClienteRepository>();

            _transaccionService = new TransaccionService(
                _transaccionRepoMock.Object,
                _cuentaRepoMock.Object
            );

            _clienteService = new ClienteService(_clienteRepoMock.Object);
        }

        [Fact]
        public async Task CrearCliente_DebeRetornarClienteCreado_CuandoDatosSonValidos()
        {
            var nuevoCliente = new Cliente
            {
                Id = 1,
                Nombre = "Carlos Ruiz",
                FechaNacimiento = new DateTime(1985, 5, 20),
                Sexo = "Masculino",
                Ingresos = 3000m,
            };

            _clienteRepoMock
                .Setup(r => r.CrearClienteAsync(It.IsAny<Cliente>()))
                .ReturnsAsync(nuevoCliente);

            var resultado = await _clienteService.CrearClienteAsync(nuevoCliente);

            resultado.Should().NotBeNull();
            resultado.Nombre.Should().Be("Carlos Ruiz");
            _clienteRepoMock.Verify(r => r.CrearClienteAsync(It.IsAny<Cliente>()), Times.Once);
        }

        [Fact]
        public async Task Deposito_DebeAumentarSaldoYRegistrarCambios()
        {
            var cuenta = new Cuenta { NumeroCuenta = "CTA-999", Saldo = 500m };
            _cuentaRepoMock.Setup(r => r.ObtenerPorNumeroAsync("CTA-999")).ReturnsAsync(cuenta);

            await _transaccionService.RealizarDepositoAsync("CTA-999", 200m);

            cuenta.Saldo.Should().Be(700m);
            _cuentaRepoMock.Verify(r => r.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task Retiro_DebeDisminuirSaldo_CuandoFondosSonSuficientes()
        {

            var cuenta = new Cuenta { NumeroCuenta = "CTA-999", Saldo = 1000m };
            _cuentaRepoMock.Setup(r => r.ObtenerPorNumeroAsync("CTA-999")).ReturnsAsync(cuenta);


            _transaccionRepoMock
                .Setup(r => r.RegistrarAsync(It.IsAny<Transaccion>()))
                .Returns(Task.CompletedTask);


            await _transaccionService.RealizarRetiroAsync("CTA-999", 400m);

            cuenta.Saldo.Should().Be(600m);
            _cuentaRepoMock.Verify(r => r.GuardarCambiosAsync(), Times.Once);
        }

        [Fact]
        public async Task AplicarIntereses_DebeCalcularMontoBasadoEnTasa()
        {
            var cuenta = new Cuenta { NumeroCuenta = "CTA-123", Saldo = 1000m };
            _cuentaRepoMock.Setup(r => r.ObtenerPorNumeroAsync("CTA-123")).ReturnsAsync(cuenta);

            await _transaccionService.AplicarInteresesAsync("CTA-123", 0.10m);

            cuenta.Saldo.Should().Be(1100m);
            _cuentaRepoMock.Verify(r => r.GuardarCambiosAsync(), Times.Once);
        }
    }
}
