# SampleDotNet6App

A multi-module .NET 6 Web API, split out from the original single-project sample. Same
feature set (products, users, JWT auth, Swagger, health checks) — now organized into
four independent modules instead of one project.

## Modules

| Module | Type | Depends on | Contains |
|---|---|---|---|
| `Domain` | Class library | — | `Product`, `User`, `PasswordHasher` |
| `Data` | Class library | `Domain` | `ApplicationDbContext`, `DataSeeder` |
| `Services` | Class library | `Domain`, `Data` | `ProductService`, `UserService`, `JwtTokenGenerator` |
| `Api` | ASP.NET Core Web API | `Domain`, `Data`, `Services` | `Program.cs`, Controllers, `appsettings*.json` |

## Build configuration

No `.sln` file. The root `SampleDotNet6App.csproj` is an aggregator — it builds nothing
itself, just references every module, the way a `.sln` would. `Directory.Build.props`
at the root sets `net6.0` and `LangVersion 10.0` once, inherited by every module.

```bash
dotnet restore SampleDotNet6App.csproj
dotnet build SampleDotNet6App.csproj
```

## Run locally

```bash
dotnet run --project Api/SampleDotNet6App.Api.csproj
```

Swagger UI: `https://localhost:7001/swagger`

### Default users (seeded)

- **Admin**: `admin` / `Admin123!`
- **Manager**: `jane.smith` / `Manager123!`
- **User**: `john.doe` / `User123!`

## API Endpoints

### Authentication
- `POST /api/users/login`
- `POST /api/users/register`
- `GET /api/users/profile`

### Products (requires authentication)
- `GET /api/products`
- `GET /api/products/{id}`
- `GET /api/products/search?q={term}`
- `POST /api/products` (Admin/Manager)
- `PUT /api/products/{id}` (Admin/Manager)
- `DELETE /api/products/{id}` (Admin)

### Users (Admin only)
- `GET /api/users`
- `GET /api/users/{id}`

### Health
- `GET /api/health`
- `GET /api/health/detailed`

## Repository layout

```
SampleDotNet6App/
├── SampleDotNet6App.csproj
├── Directory.Build.props
├── Domain/
│   ├── SampleDotNet6App.Domain.csproj
│   ├── Models/{Product.cs, User.cs}
│   └── Security/PasswordHasher.cs
├── Data/
│   ├── SampleDotNet6App.Data.csproj
│   └── Persistence/{ApplicationDbContext.cs, DataSeeder.cs}
├── Services/
│   ├── SampleDotNet6App.Services.csproj
│   ├── Abstractions/{IProductService.cs, IUserService.cs}
│   ├── Implementations/{ProductService.cs, UserService.cs}
│   └── Security/JwtTokenGenerator.cs
├── Api/
│   ├── SampleDotNet6App.Api.csproj
│   ├── Program.cs
│   ├── appsettings*.json
│   ├── Properties/launchSettings.json
│   └── Controllers/{ProductsController.cs, UsersController.cs, HealthController.cs}
└── README.md
```
