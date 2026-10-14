# OmniReserve

API backend de un sistema de gestión hotelera (habitaciones, usuarios y reservaciones), construida con **.NET 10** y **Clean Architecture**.

## Arquitectura

| Proyecto | Responsabilidad |
|---|---|
| `OmniReserve.Domain` | Entidades, enums, excepciones y Value Objects (`EmailAddress`). |
| `OmniReserve.Application` | Casos de uso con CQRS (MediatR), validaciones (FluentValidation) e interfaces de repositorios. |
| `OmniReserve.Infrastructure` | Entity Framework Core, PostgreSQL (Npgsql), repositorios y migraciones. |
| `OmniReserve.Api` | Controladores, Swagger y middleware de manejo global de excepciones. |

## Requisitos

- .NET SDK 10
- PostgreSQL
- Herramienta `dotnet-ef`: `dotnet tool install --global dotnet-ef`

## Cómo ejecutarlo

1. Configura la cadena de conexión **sin publicar tu contraseña**, con una variable de entorno (PowerShell):

   ```powershell
   $env:ConnectionStrings__OmniReserveDb = "Host=localhost;Port=5432;Database=OmniReserveDb;Username=postgres;Password=TU_PASSWORD;"
   ```

2. Aplica las migraciones:

   ```powershell
   dotnet ef database update --project OmniReserve.Infrastructure --startup-project OmniReserve.Api
   ```

3. Ejecuta la API:

   ```powershell
   dotnet run --project OmniReserve.Api
   ```

4. Abre Swagger en `http://localhost:<puerto>/swagger` (el puerto aparece en la consola).

## Endpoints

- `POST /api/rooms`: crea una habitación.
- `GET /api/rooms/{id}`: consulta una habitación por su identificador.