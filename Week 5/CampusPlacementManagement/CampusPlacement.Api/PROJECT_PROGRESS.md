# Campus Placement Management Platform
## Project Progress Tracker

**Technology Stack**
- Backend: ASP.NET Core Web API (.NET 10)
- Language: C#
- ORM: Entity Framework Core 10
- Database: Microsoft SQL Server
- Authentication: ASP.NET Core Identity + JWT Bearer
- Frontend: Angular (planned)
- Architecture: Layered Architecture

---

## Phase 1 – Project Foundation

### Step 1: Solution and project creation — COMPLETED

- Created solution: `CampusPlacementManagement.sln`
- Created Web API project: `CampusPlacement.Api`
- Framework: .NET 10
- Controller-based Web API structure.
- Verified the application runs successfully.

### Step 2: NuGet package installation — COMPLETED

Installed packages:

- Microsoft.AspNetCore.Authentication.JwtBearer – 10.0.14
- Microsoft.AspNetCore.Identity.EntityFrameworkCore – 10.0.14
- Microsoft.AspNetCore.OpenApi – 10.0.14
- Microsoft.EntityFrameworkCore.Design – 10.0.14
- Microsoft.EntityFrameworkCore.SqlServer – 10.0.14
- Swashbuckle.AspNetCore – 10.3.1

EF Core CLI:
- dotnet-ef – 10.0.14

**Pending:** Investigate and resolve the reported `System.Security.Cryptography.Pkcs` dependency vulnerability warnings before production.

### Step 3: Folder structure — COMPLETED

Created folders:

- Controllers
- Data
- Models
- DTOs
- Repositories
- Services
- Interfaces
  - Repositories
  - Services
- Enums
- Middleware
- Configurations
- Helpers

### Step 4: Database configuration — PARTIALLY COMPLETED

Configured SQL Server connection in `appsettings.json`.

Database:
- Name: `CampusPlacementDB`
- Server: `localhost`
- Authentication: Windows Authentication

Created `Enums/AccountStatus.cs` with:

- Pending
- Approved
- Rejected
- Suspended
- Inactive

Created `Models/ApplicationUser.cs`:
- Inherits from `IdentityUser`.
- Added FullName.
- Added AccountStatus.
- Added CreatedAt.
- Added UpdatedAt.

Created `Data/ApplicationDbContext.cs`:
- Inherits from `IdentityDbContext<ApplicationUser>`.
- Configured constructor with `DbContextOptions<ApplicationDbContext>`.

Registered `ApplicationDbContext` in `Program.cs` using `AddDbContext` and `UseSqlServer`.

### Step 5: ASP.NET Core Identity configuration — CODE CONFIGURED

Registered:

`AddIdentity<ApplicationUser, IdentityRole>()`

Configured:
- Minimum password length: 8.
- Requires digit.
- Requires uppercase letter.
- Requires lowercase letter.
- Requires special character.
- Unique email validation.
- Maximum failed login attempts: 5.
- Lockout duration: 15 minutes.

Configured Entity Framework Identity stores using:

`AddEntityFrameworkStores<ApplicationDbContext>()`

Registered default Identity token providers.

### Step 6: JWT configuration — CODE CONFIGURED

Added JWT settings in `appsettings.json`:

- Issuer
- Audience
- SecretKey placeholder

Configured JWT Bearer token validation in `Program.cs`:

- ValidateIssuer
- ValidateAudience
- ValidateLifetime
- ValidateIssuerSigningKey

Added authentication middleware before authorization middleware.

**Pending:**
- Replace the placeholder JWT secret with a securely generated secret.
- Move the secret to .NET User Secrets.
- Implement JWT generation during login.
- Test JWT authentication with protected endpoints.

### Step 7: SQL Server database creation — COMPLETED

Executed:

`dotnet ef database update`

Result:
- Build succeeded.
- SQL Server connection succeeded.
- Database `CampusPlacementDB` was created successfully.
- EF Core created `__EFMigrationsHistory`.

**Important:** The command reported that no migrations were found. Therefore, Identity tables such as `AspNetUsers` and `AspNetRoles` have not yet been created through migrations.

### Step 8: Initial Identity migration — PENDING

Next command:

`dotnet ef migrations add InitialIdentity`

After successfully creating the migration, execute:

`dotnet ef database update`

Expected Identity tables:

- AspNetUsers
- AspNetRoles
- AspNetUserRoles
- AspNetUserClaims
- AspNetRoleClaims
- AspNetUserLogins
- AspNetUserTokens

---

## Current Project Status

**Phase 1 – Project Foundation:** In progress.

| Task | Status |
|---|---|
| Solution and project creation | Completed |
| NuGet packages | Completed |
| Folder structure | Completed |
| ApplicationUser | Completed |
| AccountStatus enum | Completed |
| ApplicationDbContext | Completed |
| SQL Server connection | Verified |
| Identity configuration | Code configured |
| JWT validation configuration | Code configured |
| Database creation | Completed |
| Initial migration | Pending |
| Identity tables creation | Pending |
| JWT login implementation | Pending |

## Development Approach

- Complete one module at a time.
- Implement entities, DTOs, repositories, services and controllers.
- Test API operations using Postman.
- Verify authorization and business rules.
- Update this document after every completed step.
- Do not move to the next module until the current module is tested.

**Next milestone:** Create and apply the InitialIdentity migration to generate Identity tables in SQL Server.
