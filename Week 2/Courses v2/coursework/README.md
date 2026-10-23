# Courses Web App (ASP.NET Core + HTML/CSS/JavaScript + SQL Server Express)

This is a complete starter project for the course enrollment system. It stores courses, learners, enrollments, and payment attempts in SQL Server Express using Entity Framework Core.

## Requirements
- .NET 8 SDK
- SQL Server Express running locally
- Visual Studio 2022 or VS Code

## 1. Check your connection string
`appsettings.json` is already configured with:

```json
"DefaultConnection": "Server=.\\SQLEXPRESS;Database=CoursesDB;Trusted_Connection=True;TrustServerCertificate=True;"
```

If your SQL Server instance name differs, update the `Server=...` part.

## 2. Run the project
1. Extract this ZIP to a folder.
2. Open a terminal in the folder containing `CoursesWebApp.csproj`.
3. Run:
   ```powershell
   dotnet restore
   dotnet run
   ```
4. Open the local URL printed in the terminal (usually `http://localhost:xxxx`).
5. The app calls `Database.EnsureCreated()` at startup. It creates the `CoursesDB` database and tables automatically if they do not already exist. Your Windows account must have permission to create a database on the SQL Server instance.

## 3. View the database
Open SQL Server Management Studio and connect to `.` or ` .\SQLEXPRESS` (use the instance name that works on your PC). Refresh **Databases**. Expand `CoursesDB` > **Tables**. Tables include `Courses`, `Learners`, `Enrollments`, and `Payments`.

Example query:
```sql
USE CoursesDB;
SELECT * FROM dbo.Courses;
SELECT * FROM dbo.Learners;
SELECT * FROM dbo.Enrollments;
SELECT * FROM dbo.Payments;
```

## Notes
- Data is saved in SQL Server and remains after restarting the app.
- This learning version uses `EnsureCreated()` for easy setup. If you later change entity/database schemas, use EF Core migrations or recreate the development database.
- Payments are simulated only: no real payment gateway or money transfer is connected. `ClassRoomPayment` is simulated as successful; `OnlinePayment` is simulated as failed.
- If startup reports a SQL connection error, confirm the SQL Server Express service is running and the server name in `appsettings.json` is correct.
