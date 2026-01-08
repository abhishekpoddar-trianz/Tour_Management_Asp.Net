# Tour Management System - .NET 8 Migration

## Overview
This project has been successfully migrated from ASP.NET Web Forms 4.7.2 to .NET 8 using clean architecture principles.

## Architecture
The application follows clean architecture with four main layers:

- **Domain Layer**: Contains entities, interfaces, and DTOs
- **Application Layer**: Contains business logic, services, and AutoMapper configurations
- **Infrastructure Layer**: Contains data access, EF Core DbContext, and repository implementations
- **Web Layer**: ASP.NET Core Razor Pages UI

## Project Structure
```
TourManagement.sln
├── src/
│   ├── TourManagement.Domain/         # Domain entities and interfaces
│   ├── TourManagement.Application/    # Business logic and services
│   ├── TourManagement.Infrastructure/ # Data access and repositories
│   └── TourManagement.Web/            # Razor Pages UI
```

## Key Features Migrated
- Tour Management (CRUD operations)
- User Management
- Booking System
- SQL Server database with EF Core 8.0
- Async/await patterns throughout
- Dependency injection
- Structured logging with Serilog

## Database Setup
1. Update the connection string in `appsettings.json`
2. Run migrations:
   ```bash
   dotnet ef migrations add InitialCreate --project src/TourManagement.Infrastructure --startup-project src/TourManagement.Web
   dotnet ef database update --project src/TourManagement.Infrastructure --startup-project src/TourManagement.Web
   ```

## Running the Application
```bash
cd src/TourManagement.Web
dotnet run
```

Navigate to https://localhost:5001 or http://localhost:5000

## Migration Notes
- Replaced System.Web with ASP.NET Core equivalents
- Migrated ADO.NET to Entity Framework Core 8.0
- Converted Web Forms pages to Razor Pages
- Replaced ViewState with modern state management
- Implemented proper async/await patterns
- Added structured logging
- Implemented clean architecture principles

## Technologies Used
- .NET 8
- ASP.NET Core Razor Pages
- Entity Framework Core 8.0
- AutoMapper 12.0
- Serilog 8.0
- Bootstrap 5
- SQL Server

## Security Improvements
- Parameterized queries via EF Core (SQL injection prevention)
- File upload validation
- HTTPS enforcement
- Proper error handling

## Next Steps
- Consider implementing ASP.NET Core Identity for authentication
- Add unit and integration tests
- Implement caching strategies
- Add API endpoints if needed
