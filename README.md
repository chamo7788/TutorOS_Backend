# TutorOS API

ASP.NET Core Web API for managing TutorOS data. Currently includes Entity Framework Core setup for a `students` table backed by PostgreSQL.

## Prerequisites

- .NET SDK 10.0 or newer
- PostgreSQL 14+ (or a compatible managed instance such as Supabase)
- Access to a database role that can create schemas, tables, and the `pgcrypto` extension

## Configuration

1. Update `appsettings.Development.json` with a valid connection string under `ConnectionStrings:DefaultConnection`.
2. When running in other environments, use user secrets or environment variables to override the connection string.

## Database migrations

1. Ensure the EF Core CLI is available:
   ```bash
   dotnet tool install --global dotnet-ef # skip if already installed
   ```
2. Apply the latest migrations:
   ```bash
   dotnet ef database update
   ```
3. To add new migrations:
   ```bash
   dotnet ef migrations add <MigrationName>
   ```

## Running the API

```bash
 dotnet run --project TutorOS.Api.csproj
```

The service listens on the configured URLs (see `launchSettings.json`). In development it exposes OpenAPI/Swagger UI automatically.

## Project structure

- `Program.cs` – application bootstrap and service registration
- `DbContext/ApplicationDbContext.cs` – EF Core context with model registrations
- `Models/Student.cs` – entity definition aligned with the `students` table
- `Configurations/StudentConfiguration.cs` – fluent configuration for defaults, constraints, and indexes
- `Migrations/` – generated EF Core migrations for database schema changes

## Troubleshooting

- If you encounter `gen_random_uuid()` errors, ensure the database role can create the `pgcrypto` extension or manually enable it.
- For SSL certificate issues, set `Trust Server Certificate=true` only in development and prefer valid certificates in production.
