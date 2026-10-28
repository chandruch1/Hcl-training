# Bus Ticket Booking Console Application

## 1. Project Overview

**Project Name:** Bus Ticket Booking Console Application  
**IDE:** Visual Studio  
**Framework:** .NET 10  
**Language:** C#  
**Database:** SQL Server Express  
**Data Access:** ADO.NET  
**Version Control:** Git  

### Project Objective

Develop a console-based bus ticket booking application using C#, ADO.NET, and SQL Server. The application will support passenger management, bus and route management, trip scheduling, seat selection, ticket booking, payments, and booking history.

---

## 2. Technology Stack

- C# and .NET 10
- SQL Server Express
- ADO.NET with `Microsoft.Data.SqlClient`
- Visual Studio
- Git and GitHub

---

## 3. Development Progress

### Setup and Configuration

- [x] Installed the required development environment.
- [x] Created the project in Visual Studio.
- [x] Created the `BusTicketDB` database in SQL Server Express.
- [x] Configured the SQL Server connection string.
- [x] Tested the database connection successfully.

### Feature 1: Project Setup and Database Connection

**Status:** Completed

The application connects to SQL Server Express using Windows Authentication.

Connection string:

```text
Server=.\SQLEXPRESS;Database=BusTicketDB;Trusted_Connection=True;TrustServerCertificate=True;
```

**Note:** The database must exist before connecting with this connection string. Automatic database creation will be implemented in a later feature.

### Feature 2: Domain Models

**Status:** In progress — build and Git commit confirmation pending.

The following model classes are planned for the `Models` folder:

| No. | Model | Purpose |
|---:|---|---|
| 1 | `Passenger.cs` | Represents a passenger |
| 2 | `Bus.cs` | Represents a bus |
| 3 | `Route.cs` | Represents a travel route |
| 4 | `Trip.cs` | Represents a scheduled trip |
| 5 | `Seat.cs` | Represents a seat on a bus |
| 6 | `Booking.cs` | Represents a passenger booking |
| 7 | `BookingSeat.cs` | Links bookings to selected seats |
| 8 | `Payment.cs` | Represents payment information |
| 9 | `AuditLog.cs` | Records important application actions |
| 10 | `TripSeat.cs` | Tracks seat status for a specific trip |

**Completion checklist:**

- [ ] Create all 10 model classes.
- [ ] Build the solution successfully.
- [ ] Fix any compilation errors.
- [ ] Commit the completed models feature.
- [ ] Record the Git commit hash.

---

## 4. Git Commit History

| Feature | Commit Message | Status |
|---|---|---|
| Project setup and database connection | `chore: initialize bus ticket booking project` | Commit status not confirmed |
| Domain models | `feat: add bus booking domain models` | Pending |

Only mark a commit as completed after verifying it in Git.

---

## 5. Upcoming Features

The following is the planned development order.

- [ ] **Feature 3:** Database initialization and automatic table creation using ADO.NET.
- [ ] **Feature 4:** Passenger management.
- [ ] **Feature 5:** Bus management.
- [ ] **Feature 6:** Route management.
- [ ] **Feature 7:** Trip scheduling and management.
- [ ] **Feature 8:** Seat availability management.
- [ ] **Feature 9:** Ticket booking and seat selection.
- [ ] **Feature 10:** Booking cancellation.
- [ ] **Feature 11:** Payment simulation.
- [ ] **Feature 12:** Booking history and reports.
- [ ] **Feature 13:** Audit logging and final testing.

---

## 6. Development Workflow

For every feature:

1. Understand the requirement.
2. Implement the feature in Visual Studio.
3. Build and test the code.
4. Fix any errors.
5. Commit the completed feature using a professional Git commit message.
6. Update this progress document.
7. Proceed to the next feature only after confirming completion.

---

## 7. Important Technical Notes

- C# model classes define the application's data structures; they do not automatically create SQL Server tables when using ADO.NET.
- Database tables will be created through SQL commands executed using ADO.NET.
- Database initialization should create the database and tables when needed without deleting existing data on every application run.
- Booking operations should validate seat availability and use database transactions to prevent inconsistent bookings.
- Payment processing in this project will be simulated; no real payment gateway is planned.

---

## 8. Current Next Step

**Complete Feature 2 — Domain Models.**

Verify that all 10 model classes exist and the solution builds successfully. Then commit the feature and update this document with the actual commit hash.

After that, implement **Feature 3 — Database Initialization and Automatic Table Creation using ADO.NET**.

### Feature 4: Passenger Management

**Status:** Completed

- [x] Register passengers
- [x] Retrieve all passengers
- [x] Search passengers by ID
- [x] Search passengers by email
- [x] Update passenger details
- [x] Deactivate passengers
- [x] Retrieve active passengers
- [x] Validate passenger input
- [x] Build and test successfully

**Commit message:** `feat: implement passenger management repository`

**Next feature:** Bus Management using C# and ADO.NET.