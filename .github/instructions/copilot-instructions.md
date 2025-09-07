# GitHub Copilot Instructions for this Repository

## General Guidelines
- This project uses **ABP Framework** and follows **Layered Architecture with Clean Architecture/DDD principles**.
- Always respect separation of concerns between layers.
- Do not mix infrastructure logic with domain logic.
- Use ABP conventions: ApplicationService, AbpController, Dependency Injection, DTOs, Repositories.
---

## Layer Rules

### Domain Layer
- Define **Entities** inheriting from `AggregateRoot<Guid>` or `Entity<Guid>`.
- Keep **business logic** inside entities when possible.
- Define **Repository interfaces** (e.g., `IUserRepository : IRepository<User, Guid>`).
- Do NOT depend on EF Core directly.

### EntityFrameworkCore Layer
- Configure `DbContext` (inherits from `AbpDbContext`).
- Implement repository interfaces defined in Domain layer.
- Register entities via `DbSet<TEntity>`.
- Keep persistence concerns here only.
### Application.Contracts Layer
- Define **DTOs** for input/output.
- Define **Service Interfaces** (e.g., `IUserAppService : IApplicationService`).
- Keep this layer free of logic — just contracts and DTO definitions.

### Application Layer
- Implement **Application Services** (inherit from `ApplicationService`).
- Use AutoMapper profiles for mapping between Entities and DTOs.
- Application services should orchestrate domain and infrastructure but NOT contain persistence logic directly.

### HttpApi Layer
- Define **Controllers** inheriting from `AbpController`.
- Expose Application services via REST endpoints.
- Keep controllers thin (only call application services).

### DbMigrator
- Implement schema migration logic.
- Run `Add-Migration` and `Update-Database` for applying migrations.
- Do not add business logic here.

### Host Project (Startup/Program)
- Configure ABP modules, dependency injection, and middleware.
- Do not place domain or application logic here.

---

## Coding Style
- Use `async/await` everywhere for DB or IO operations.
- Follow ABP naming conventions:
  - DTOs end with `Dto` (e.g., `UserDto`, `CreateUserDto`).
  - Application services end with `AppService`.
  - Repository interfaces prefixed with `I` and end with `Repository`.
- Use AutoMapper for entity ↔ DTO conversion.
- Write clean, maintainable, and self-documented code.

---