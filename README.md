# SportsStore

SportsStore is an ASP.NET Core MVC application for running a sports retail shop: catalog, stock, customer orders, and store administration.

The solution is split so the web interface, business rules, and database access can change independently:

- `src/SportsStore.Web` — Razor pages, controllers, and the public storefront
- `src/SportsStore.Services` — business logic
- `src/SportsStore.Data` — Entity Framework Core and SQL Server
- `src/SportsStore.Core` — domain entities shared by the other projects
- `tests/SportsStore.Tests` — automated tests for the business logic

## Requirements

- .NET 8 SDK
- SQL Server LocalDB (included with Visual Studio / SQL Server Express LocalDB)

## Run the storefront

```bash
dotnet run --project src/SportsStore.Web
```

The database connection string is in `src/SportsStore.Web/appsettings.json`.
