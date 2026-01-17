using BankApi.Application.Interfaces;
using BankApi.Application.Services;
using BankApi.Infrastructure.Database;
using BankApi.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<BankDbContext>(opt => opt.UseSqlite("Data Source=bank.db"));

builder.Services.AddEndpointsApiExplorer();


// Repositorios
builder.Services.AddScoped<ICuentaRepository, CuentaRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<ITransaccionRepository, TransaccionRepository>();

// Servicios
builder.Services.AddScoped<CuentaService>();
builder.Services.AddScoped<ClienteService>();
builder.Services.AddScoped<TransaccionService>();


builder.Services.AddControllers();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseAuthorization(); 
app.MapControllers();

app.Run();