API Bancaria - Examen Final

Esta es una API desarrollada en **.NET 8** que permite la gestión de clientes, cuentas y transacciones bancarias (Depósitos, Retiros e Intereses).

Requisitos

- .NET 8 SDK
- SQLite

Para levantar la API, ejecuta:

dotnet run

El proyecto incluye una suite de pruebas para validar la lógica de negocio (Saldos, Retiros insuficientes, etc.).

Para ejecutar las pruebas:
cd Test
dotnet test
