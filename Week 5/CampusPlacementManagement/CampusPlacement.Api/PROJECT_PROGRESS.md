# Campus Placement Management Platform
## Project Development Progress

**Technology Stack**
- Backend: ASP.NET Core Web API (.NET 10)
- Language: C#
- Database: Microsoft SQL Server
- ORM: Entity Framework Core 10
- Authentication: ASP.NET Core Identity + JWT
- Frontend: Angular (Future)

**Architecture:** Controller → Service → Repository → DbContext → SQL Server

---

## Phase 1: Project Foundation

### Step 1: Project Creation

**Status:** Completed

**Implemented:**
- Created solution: `CampusPlacementManagement`
- Created Web API project: `CampusPlacement.Api`
- Configured Controller-based Web API.
- Added API project to the solution.
- Executed the application successfully.

**Project Structure:**
```text
CampusPlacementManagement/
├── CampusPlacementManagement.sln
└── CampusPlacement.Api/
    ├── Controllers/
    ├── Program.cs
    ├── appsettings.json
    └── CampusPlacement.Api.csproj
```

**Testing:**
- Command: `dotnet run`
- Result: Successful
- Local URL: http://localhost:5169
- Environment: Development

**Concepts Learned:**
- What is a .NET solution?
- What is a Web API project?
- Difference between Minimal API and Controller-based API.
- How to run an ASP.NET Core application.

**Next Step:** Install required NuGet packages.

### Step 2: NuGet Package Installation

**Status:** In Progress – JWT and CLI verification pending.

**Successfully Installed Packages:**

| Package | Version |
|---|---|
| Microsoft.EntityFrameworkCore.SqlServer | 10.0.14 |
| Microsoft.EntityFrameworkCore.Design | 10.0.14 |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 10.0.14 |
| Swashbuckle.AspNetCore | 10.3.1 |
| Microsoft.AspNetCore.OpenApi | 10.0.14 |

**Build Verification:**
- Command: `dotnet list package`
- Result: Build succeeded.
- Warnings: NU1902 and NU1903 for System.Security.Cryptography.Pkcs 9.0.11.

**Pending Tasks:**
- Install Microsoft.AspNetCore.Authentication.JwtBearer.
- Install and verify dotnet-ef 10.0.14.
- Investigate the reported dependency vulnerability.

**Next Step:** Complete package verification, then create the layered architecture folder structure.


### Step 2: NuGet Package Installation

**Status: COMPLETED**

**Installed Packages:**

| Package | Version |
|---|---|
| Microsoft.EntityFrameworkCore.SqlServer | 10.0.14 |
| Microsoft.EntityFrameworkCore.Design | 10.0.14 |
| Microsoft.AspNetCore.Identity.EntityFrameworkCore | 10.0.14 |
| Microsoft.AspNetCore.Authentication.JwtBearer | 10.0.14 |
| Microsoft.AspNetCore.OpenApi | 10.0.14 |
| Swashbuckle.AspNetCore | 10.3.1 |

**EF Core CLI:**
- Installed version: 10.0.14
- Verification: `dotnet ef --version`
- Result: Successful.

**Build Verification:**
- Command: `dotnet list package`
- Result: Build succeeded.
- Remaining warnings: NU1902 and NU1903 related to System.Security.Cryptography.Pkcs 9.0.11.
- Security investigation pending before production deployment.

**Concepts Learned:**
- NuGet package management.
- EF Core SQL Server provider.
- Entity Framework migrations.
- ASP.NET Core Identity.
- JWT Bearer authentication.
- EF Core CLI.

**Next Step:** Create layered architecture folder structure.


### Step 4: SQL Server Connection Configuration

**Status: CONFIGURATION COMPLETED – CONNECTION TEST PENDING**

**Database:** CampusPlacementDB

**Configuration File:** appsettings.json

**Connection Details:**
- Server: localhost
- Database: CampusPlacementDB
- Authentication: Windows Authentication
- TrustServerCertificate: True

**Concepts Learned:**
- Connection strings
- Configuration in appsettings.json
- Windows Authentication
- SQL Server connection properties

**Pending:**
- Verify SQL Server instance.
- Create ApplicationDbContext.
- Register DbContext in Program.cs.
- Test database connectivity.

**Next Step:** Create ApplicationDbContext.

### Step 4: ApplicationDbContext and Identity Base Setup

**Status: IMPLEMENTATION COMPLETED – BUILD VERIFICATION PENDING**

**Files Created:**
1. Models/ApplicationUser.cs
2. Enums/AccountStatus.cs
3. Data/ApplicationDbContext.cs

**Implementation Details:**

- ApplicationUser inherits from IdentityUser.
- Added FullName, AccountStatus, CreatedAt and UpdatedAt.
- Created AccountStatus enum with Pending, Approved, Rejected, Suspended and Inactive.
- ApplicationDbContext inherits from IdentityDbContext<ApplicationUser>.
- Configured SQL Server using UseSqlServer.
- Registered ApplicationDbContext through AddDbContext in Program.cs.

**Concepts Learned:**
- Inheritance in C#.
- ASP.NET Core Identity.
- IdentityUser versus ApplicationUser.
- IdentityDbContext versus DbContext.
- Entity Framework Core DbContext.
- Dependency Injection and scoped DbContext lifetime.
- Connection string configuration.

**Testing:**
- Pending: dotnet build.
- Pending: Verify SQL Server connection during migration.

**Next Step:** Configure Identity services and prepare the initial database migration.

