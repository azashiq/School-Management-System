# School Management System

A production-ready School Management System built with ASP.NET Core MVC, Entity Framework Core, and SQL Server, following Clean Architecture (Domain, Infrastructure, Web).

## Requirements
- Visual Studio 2026 (or Visual Studio 2022 17.8+)
- .NET 8 SDK
- SQL Server / SQL Server LocalDB

## Getting Started
1. Open `SchoolManagementSystem.sln` in Visual Studio 2026.
2. Restore NuGet packages (happens automatically on build).
3. Update the connection string in `SchoolManagementSystem.Web/appsettings.json` if needed. The default targets LocalDB:
   `Server=(localdb)\mssqllocaldb;Database=SchoolManagementSystemDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True`
4. Set `SchoolManagementSystem.Web` as the startup project.
5. Open the Package Manager Console, select `SchoolManagementSystem.Infrastructure` as the default project, and run:
   ```
   Add-Migration InitialCreate -Project SchoolManagementSystem.Infrastructure -StartupProject SchoolManagementSystem.Web
   ```
   (The app also calls `context.Database.Migrate()` automatically at startup, so once a migration exists it will apply and seed demo data on first run.)
6. Press F5 to run. The app opens on the Students list.

## Architecture
- **SchoolManagementSystem.Domain** — Entities, enums, and the `IRepository<T>` abstraction. No dependencies on EF Core or ASP.NET.
- **SchoolManagementSystem.Infrastructure** — `ApplicationDbContext`, Fluent API entity configurations, the generic `Repository<T>`, and DI wiring (`AddInfrastructure`).
- **SchoolManagementSystem.Web** — ASP.NET Core MVC controllers and Razor views (Bootstrap 5 UI) for Students, Teachers, Classrooms, Subjects, Attendance, and Grades.

## Key Features
- Clean Architecture with clear separation of concerns.
- Soft delete (`IsDeleted`, `DeletedAt`) enforced via EF Core global query filters, so deleted records never appear in query results.
- Generic repository pattern (`IRepository<T>` / `Repository<T>`) used by every controller — no duplicated data-access code.
- Full CRUD (Create, Read, Update, Delete) for all six modules: Student, Teacher, ClassRoom, Subject, Attendance, Grade.
- Fluent API configurations: max lengths, unique indexes (registration number, employee ID, subject code), decimal precision for grade scores, and `Restrict`/`Cascade` delete behaviors matched to the domain rules.
- Responsive Bootstrap 5 UI with search, badges for attendance status, and dismissible success/error alerts.
- A `DbSeeder` seeds two classrooms, two teachers, two subjects, and two students on first run for demo purposes.
