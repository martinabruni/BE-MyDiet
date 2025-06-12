# MyDiet Backend

This repository contains the backend services for the **MyDiet** application. The solution is built with **.NET 8** and follows a Domain Driven Design approach. It is organised into four main projects:

- **Apis** – ASP.NET Core web API exposing the application endpoints.
- **Businesses** – application services and domain logic.
- **Domains** – DTOs and interfaces shared across the solution.
- **Infrastructures** – database access, security helpers and other integrations.

The database schema is defined in [`src/Infrastructures/schema.md`](src/Infrastructures/schema.md) and maps entities such as `User`, `Diet`, `Plan`, `Meal` and other related tables.

## Getting started

1. Install the [.NET 8 SDK](https://dotnet.microsoft.com/download).
2. Restore dependencies and build the solution:

   ```bash
   dotnet restore MyDiet.sln
   dotnet build MyDiet.sln
   ```

3. Run the API:

   ```bash
   dotnet run --project src/Apis/MyDiet.Core.Api
   ```

   The API includes endpoints for obtaining a public key and generating JWT tokens as defined in `UserJwtTokenController`.

A `Dockerfile` is also provided under `src/Apis/MyDiet.Core.Api` for containerised deployments.

## Project structure

```
src/
├── Apis/
│   └── MyDiet.Core.Api/          # ASP.NET Core API
├── Businesses/
│   └── MyDiet.Core.Business/     # Domain services
├── Domains/
│   └── MyDiet.Core.Domain/       # DTOs and shared interfaces
└── Infrastructures/
    ├── MyDiet.Core.Database/     # SQL Server project
    ├── MyDiet.Core.Security/     # JWT helpers and key providers
    └── MyDiet.Core.Sql/          # EF Core models and repositories
```

The `MyDietCoreDbContext` class configures the EF Core mappings for all entities, while generic repositories implement basic CRUD operations.

## License

This project is provided as a starting point for a diet planning backend. Feel free to adapt it to your needs.
