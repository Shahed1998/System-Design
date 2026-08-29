# Modular Monolith — eCommerce (SCEcommerz)

A reference implementation of a **modular monolith** architecture in .NET 10, built as an eCommerce backend. Each business capability (Catalog, Basket, Order) lives in its own independently-referenced module with its own database context, yet all modules are hosted and deployed as a single ASP.NET Core application.

## Architecture

The solution is organized around three architectural pillars:

- **Modular Monolith** — Business capabilities are isolated into self-contained modules under `Modules/`. Each module exposes its services through a single extension-method entry point (`Add<Module>Module` / `Use<Module>Module`), keeping module wiring explicit and the composition root (`Bootstrapper/API`) thin.
- **Vertical Slice Architecture** — Inside each module, functionality is organized by feature rather than by technical layer. Each feature under `Products/Features/*` is self-contained: it owns its endpoint, command/query, handler, and validator.
- **CQRS + Domain-Driven Design** — Commands and queries are dispatched through MediatR (`ICommand`/`IQuery` in the `Shared` project), and domain entities are modeled as DDD aggregates that raise domain events on state changes.

```
┌─────────────────────────────────────────────┐
│             Bootstrapper/API                │
│        (composition root, Carter, OpenAPI)  │
└───────────────┬───────────────┬──────────────┘
                │               │
     ┌──────────┴───┐  ┌───────┴───────┐   ┌───────────────┐
     │    Catalog   │  │    Basket     │   │     Order     │
     │    module    │  │    module     │   │    module     │
     └──────┬───────┘  └───────────────┘   └───────────────┘
            │
     ┌──────┴────────┐
     │    Shared      │
     │ (CQRS, DDD,    │
     │EF interceptors,│
     │  Carter/valid. │
     │ extensions)    │
     └────────────────┘
```

### Solution layout

```
Bootstrapper/
  API/                      Composition root: Program.cs wires up all modules, Carter, OpenAPI/Swagger
Modules/
  Catalog/Catalog/          Product catalog module (fully implemented)
    Data/                   EF Core DbContext, configurations, migrations, seeders
    EventHandlers/          Domain event handlers (e.g. product created / price changed)
    Products/
      Models/               Product aggregate
      Events/                Domain events
      DTOs/                  Data transfer objects
      Features/              One folder per vertical slice (CreateProduct, GetProducts,
                              GetProductById, GetProductByCategory, UpdateProduct, DeleteProduct)
  Basket/Basket/            Basket module (scaffolded, not yet implemented)
  Order/Order/              Order module (scaffolded, not yet implemented)
Shared/Shared/
  CQRS/                     ICommand, IQuery, and handler abstractions built on MediatR
  DDD/                      Entity/Aggregate base classes, domain event contracts
  Behaviors/                MediatR pipeline behaviors (FluentValidation)
  Data/                     EF Core save-changes interceptors, migration/seeding extensions
  Extensions/               Carter module auto-registration
docker-compose.yml / .override.yml   PostgreSQL container for local development
```

## Tech stack

| Concern              | Technology                                   |
|----------------------|-----------------------------------------------|
| Runtime              | .NET 10 / ASP.NET Core                        |
| API surface          | [Carter](https://github.com/CarterCommunity/Carter) (minimal-API modules) |
| CQRS / mediation     | [MediatR](https://github.com/jbogard/MediatR) |
| Validation           | [FluentValidation](https://fluentvalidation.net/) via a MediatR pipeline behavior |
| Object mapping       | [Mapster](https://github.com/MapsterMapper/Mapster) |
| Persistence          | Entity Framework Core + [Npgsql](https://www.npgsql.org/) |
| Database             | PostgreSQL 17 (Dockerized)                    |
| API docs             | ASP.NET Core OpenAPI + Swagger UI              |

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker](https://www.docker.com/) (for PostgreSQL)

### 1. Start the database

```bash
docker compose up -d
```

This starts a PostgreSQL 17 container (`eshopDB`) exposed on host port `6000`, with database `EShopDb` and credentials `postgres` / `postgres` (see `docker-compose.override.yml`).

### 2. Run the API

```bash
dotnet run --project Bootstrapper/API
```

On startup, each module applies its pending EF Core migrations and seeds initial data automatically (see `UseCatalogModule` → `Shared.Data.Extensions.UseMigration`).

The API listens per `Bootstrapper/API/Properties/launchSettings.json`. In `Development`, Swagger UI is available at `/swagger` (served from the generated `/openapi/v1.json` document).

### Running with Docker

A `Dockerfile` is provided under `Bootstrapper/API/` for containerized builds:

```bash
docker build -f Bootstrapper/API/Dockerfile -t modular-monolith-api .
```

## Catalog module — API reference

The Catalog module is the only fully implemented module today. It exposes a REST API for managing products.

| Method | Route                              | Description                          |
|--------|-------------------------------------|---------------------------------------|
| GET    | `/products`                         | List all products                     |
| GET    | `/products/{id}`                    | Get a product by ID                   |
| GET    | `/products/category/{category}`     | Get products by category              |
| POST   | `/products`                         | Create a new product                  |
| PUT    | `/products`                         | Update an existing product            |
| DELETE | `/products/{id}`                    | Delete a product                      |

Each endpoint is a `ICarterModule` living alongside its MediatR command/query and handler under `Modules/Catalog/Catalog/Products/Features/`, following the vertical-slice pattern.

### Domain model

`Product` is a DDD aggregate (`Aggregate<Guid>`) that enforces invariants through factory/update methods and raises domain events:

- `ProductCreatedEvent` — raised when a product is created
- `ProductPriceChangeEvent` — raised when an existing product's price changes

These events are dispatched via an EF Core `SaveChangesInterceptor` (`DispatchDomainEventsInterceptor`) and handled by MediatR notification handlers in `Modules/Catalog/Catalog/EventHandlers/`.

## Shared building blocks

The `Shared` project contains cross-cutting infrastructure consumed by every module:

- **CQRS contracts** (`ICommand`, `ICommand<TResponse>`, `IQuery<TResponse>`, and matching handler interfaces) that constrain MediatR usage to a consistent command/query shape.
- **DDD primitives** (`Entity<TId>`, `Aggregate<TId>`, `IDomainEvent`) shared by all module domain models.
- **`ValidationBehavior<TRequest, TResponse>`** — a MediatR pipeline behavior that runs all registered FluentValidation validators for a command and short-circuits with a `ValidationException` on failure.
- **EF Core interceptors** — `AuditableEntityInterceptor` (created/updated timestamps) and `DispatchDomainEventsInterceptor` (publishes aggregate domain events through MediatR on `SaveChanges`).
- **`AddCarterWithAssemblies`** — registers Carter modules discovered by reflection from a given set of assemblies, so each module can register its own endpoints without the host needing to know about them individually.
- **`UseMigration<TContext>`** — applies pending EF Core migrations and runs any registered `IDataSeeder` implementations at startup.

## Project status

- ✅ **Catalog** — implemented (CRUD endpoints, EF Core persistence, domain events, seeding)
- 🚧 **Basket** — module scaffolded, no functionality yet
- 🚧 **Order** — module scaffolded, no functionality yet

## Adding a new module

1. Create a class library under `Modules/<ModuleName>/<ModuleName>/` referencing `Shared`.
2. Add `Add<ModuleName>Module(IServiceCollection, IConfiguration)` and `Use<ModuleName>Module(IApplicationBuilder)` extension methods, following the pattern in `CatalogModule.cs`.
3. Register the module's assembly in `AddCarterWithAssemblies` and wire the two extension methods into `Bootstrapper/API/Program.cs`.
4. Organize features as vertical slices under `<ModuleName>/Features/<FeatureName>/`, each with an endpoint, a command/query + handler, and (optionally) a FluentValidation validator.
