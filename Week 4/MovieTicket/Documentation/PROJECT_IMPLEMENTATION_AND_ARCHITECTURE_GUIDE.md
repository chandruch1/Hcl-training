# Movie Ticket Booking System — Complete Implementation & Architecture Guide

---

## 📌 Executive Summary

This document provides an end-to-end breakdown of the **Movie Ticket Booking System** built using **ASP.NET Core Web API (.NET 10)**, **Entity Framework Core 10**, and **SQL Server**. It explains the purpose, technical rationale, and code interactions across all 11 development phases.

---

## 🏛️ System Architecture

The application strictly follows the Clean Layered Architecture:

```
[ HTTP Client / Postman / Angular ]
               │
               ▼
       1. Controllers
  (Routing, HTTP status, DTO validation)
               │
               ▼
        2. Services
  (Business logic, validation, calculations)
               │
               ▼
       3. Repositories
  (Data access, EF queries, Transactions)
               │
               ▼
     4. ApplicationDbContext
  (EF Core ORM, Fluent API, Mapping)
               │
               ▼
         5. SQL Server
       (Tables, Indexes, Foreign Keys)
```

### Architectural Principles Applied
1. **Separation of Concerns:** Controllers do not touch the database; Repositories do not make business decisions; Services orchestrate business logic.
2. **Thin Controllers:** Controllers only receive HTTP requests, delegate work to the corresponding service, and return HTTP status codes with response DTOs.
3. **No Entity Exposure:** Raw EF Core database entities are never returned to the API clients. Instead, typed **DTOs** (Data Transfer Objects) are used for all request inputs and response outputs.
4. **Non-Generic Repositories:** Each aggregate root entity has its own dedicated repository interface and implementation to prevent leaky abstractions.
5. **Atomic Transactions:** Multi-step workflows (like show scheduling and booking creation) run within database transactions (`IDbContextTransaction`) to ensure ACID compliance.

---

## 🔄 Detailed Breakdown of Phases (1 to 11)

### Phase 1: Solution Setup & SQL Server Configuration
* **Objective:** Establish the foundational Web API project, configure dependency injection, and set up the SQL Server connection.
* **Key Actions:**
  * Created `MovieTicketBooking` ASP.NET Core Web API targeting `.NET 10.0`.
  * Installed NuGet packages:
    * `Microsoft.EntityFrameworkCore.SqlServer` (v10.0.0) — SQL Server database provider.
    * `Microsoft.EntityFrameworkCore.Tools` (v10.0.0) — Migration and CLI tooling.
    * `Swashbuckle.AspNetCore` (v10.3.1) — Swagger / OpenAPI documentation generator.
  * Configured connection string in `appsettings.json` pointing to database `MovieTicketBookingDB`.
  * Initialized `ApplicationDbContext` and registered it with `Scoped` lifetime in `Program.cs`.

### Phase 2: Domain Entity Models
* **Objective:** Define clean, strongly-typed C# domain models representing the movie ticketing domain.
* **Entities Created:**
  1. `Movie`: Stores movie catalog data (Title, DurationMinutes, Rating, Genre, Language, ReleaseDate).
  2. `Theatre`: Physical cinema venue (Name, Address, City, State, Phone, Email).
  3. `Screen`: Cinema hall inside a theatre (ScreenName, TotalSeats, ScreenType like IMAX, 4DX).
  4. `Seat`: Physical seat inside a screen (RowLetter, ColumnNumber, SeatType, Price).
  5. `Show`: Scheduled screening of a movie on a screen (ShowDate, StartTime, EndTime, BasePrice).
  6. `ShowSeat`: Represents the availability of a specific seat for a given show (`Available`, `Booked`, `Blocked`).
  7. `Booking`: Customer ticket purchase order (CustomerName, CustomerEmail, TotalAmount, BookingStatus).
  8. `BookingSeat`: Join entity between Booking, Seat, and ShowSeat recording the historical price paid.
  9. `ShowSeatStatus` & `BookingStatusConstants`: Strongly typed string constants to eliminate magic strings.

### Phase 3: Fluent API Relationships, Constraints & Indexes
* **Objective:** Configure schema rules, cascade policies, and performance indexes in `ApplicationDbContext.OnModelCreating`.
* **Important Database Constraints:**
  * **Composite Unique Index on Seats:** `(ScreenId, RowLetter, ColumnNumber)` is unique. Prevents duplicate seat coordinates (e.g., two `A1` seats in Screen 1).
  * **Composite Unique Index on ShowSeats:** `(ShowId, SeatId)` is unique. A seat can only appear once per show schedule.
  * **Search Index:** `IX_Movie_Title` on `Movie.Title` for fast title lookups.
  * **Schedule Query Index:** `IX_Show_Screen_Date` on `(ScreenId, ShowDate)` for quick conflict lookups.
  * **Delete Behaviors:** Configured `DeleteBehavior.Restrict` on all parent-child relationships to prevent accidental data deletion (e.g., deleting a Theatre does not wipe its historical screens or bookings). Only `BookingSeat -> Booking` uses `Cascade`.

### Phase 4: EF Core Migrations & Database Creation
* **Objective:** Generate reproducible migration scripts and apply them directly to SQL Server.
* **Key Actions:**
  * Generated initial migration `InitialCreate` using `dotnet ef migrations add InitialCreate`.
  * Executed `dotnet ef database update`, creating database `MovieTicketBookingDB` with all tables, constraints, foreign keys, and indexes in SQL Server.

### Phase 5: Movie Module (CRUD, Pagination, Search)
* **Objective:** Build robust movie management APIs.
* **Features:**
  * Server-side pagination with `page` and `pageSize` parameters returning total records and total page counts.
  * Substring title search via LINQ `Contains()`.
  * Read-only optimization using `.AsNoTracking()` to reduce EF Core memory tracking overhead.

### Phase 6: Theatre, Screen & Seat Modules
* **Objective:** Manage venue hierarchies from the cinema chain down to individual seats.
* **Features:**
  * Theatres and screens with eager loading via `.Include(s => s.Theatre)`.
  * Single seat creation and bulk seat generation (`POST /api/seats/bulk`).
  * Backend validation against coordinate conflicts before saving.

### Phase 7: Show Scheduling & Auto-Seat Generation
* **Objective:** Allow scheduling of movie shows while enforcing business rules.
* **Core Business Logic:**
  1. **Overlap Prevention:** Checks if `StartTime < existingEnd && EndTime > existingStart` for the same screen on the same date. Overlapping schedules are rejected with `400 Bad Request`.
  2. **Automatic ShowSeat Generation:** When a show is saved, the service queries all physical seats in that screen and automatically creates `ShowSeat` records initialized with status `Available`.

### Phase 8: Booking Management & Atomic Transactions
* **Objective:** Handle ticket reservation and cancellation reliably.
* **Core Business Logic:**
  1. **Server-Side Price Calculation:** The frontend never decides the price. The backend fetches active seat prices and computes `TotalAmount`.
  2. **Availability Check:** Verifies that all requested seats belong to the selected show's screen and currently have `Available` status.
  3. **Atomic Commit:** Booking creation, booking seat recording, and updating show seat statuses to `Booked` are executed within an `IDbContextTransaction`. If any step fails, everything rolls back.
  4. **Preserved History Cancellation:** When cancelled, booking records are **never deleted**. Their status changes to `Cancelled`, and the associated `ShowSeat` rows revert to `Available`.

### Phase 9: Global Exception Handling & Logging
* **Objective:** Centralize error handling so controllers don't need repetitive `try-catch` blocks.
* **Implementation:** `ExceptionHandlingMiddleware` catches:
  * `KeyNotFoundException` $\rightarrow$ `404 Not Found`
  * `InvalidOperationException` $\rightarrow$ `400 Bad Request`
  * `ArgumentException` $\rightarrow$ `400 Bad Request`
  * Unhandled errors $\rightarrow$ `500 Internal Server Error`
* Structured logs are output via `ILogger<T>`.

### Phase 10: Complete Integration Testing
* **Objective:** Verify end-to-end communication from HTTP requests to database commits.
* Probed all endpoints through an automated test harness to ensure zero errors and correct HTTP status codes (`201 Created`, `200 OK`, `204 No Content`).

### Phase 11: Angular Integration Readiness
* **Objective:** Ensure the backend is plug-and-play for an Angular frontend.
* Configured CORS policy `"AllowAngular"` allowing requests from `http://localhost:4200` with full methods, headers, and credentials.
* DTO serialization conforms to `camelCase` standard.

---

## 🔌 Dependency Injection Lifecycle Summary

All services and repositories are registered with `Scoped` lifetime in `Program.cs`:

```csharp
// Repositories
builder.Services.AddScoped<IMovieRepository, MovieRepository>();
builder.Services.AddScoped<ITheatreRepository, TheatreRepository>();
builder.Services.AddScoped<IScreenRepository, ScreenRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
builder.Services.AddScoped<IShowRepository, ShowRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Services
builder.Services.AddScoped<IMovieService, MovieService>();
builder.Services.AddScoped<ITheatreService, TheatreService>();
builder.Services.AddScoped<IScreenService, ScreenService>();
builder.Services.AddScoped<ISeatService, SeatService>();
builder.Services.AddScoped<IShowService, ShowService>();
builder.Services.AddScoped<IBookingService, BookingService>();
```

### Why `Scoped`?
* `ApplicationDbContext` is scoped per HTTP request.
* Repositories and Services share the same `DbContext` instance for the lifetime of that single request.
* Once the HTTP response is sent, all allocated resources and database connections are automatically disposed.
