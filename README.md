# NATO User Transaction App

## Project Identification

**Project Title:** NATO – Transaction Management & Summary API

**Description:**  
TransactionApp is a .NET 8 Web API for managing users and their transactions, with powerful summary and reporting features.
It supports CRUD operations, high-volume transaction detection, and efficient in-memory aggregation for summaries.

---

## Project Overview

This Transaction Management API is built using ASP.NET Core, Entity Framework Core, and SQL Server to handle user and transaction data.
The system provides endpoints for creating, retrieving, and summarizing transactions, with support for pagination and in-memory caching to optimize performance.

### Caching Mechanism

  *  We use in-memory caching for two heavy summary endpoints:
      *  Total transactions per user
      *  Total transactions per transaction type
   
  *  These summaries are pre-computed and stored in memory for a configurable duration, avoiding repeated database hits for the same data.
  *  The cache duration is fully configurable via appsettings.json under the CacheSettings section, allowing flexible tuning without code changes.
  *  If the database is small, this in-memory approach is extremely fast since all calculations happen in RAM without re-querying the database.

### Pagination Mechanism
  *  All list-based endpoints (e.g., GetAll Users, GetAll Transactions, High Volume Transactions) use server-side pagination directly in the database query.
  *  This ensures that only the required subset of data is fetched from the database, improving query performance and reducing memory usage on the API server.

---

## Visuals
<img width="1541" height="896" alt="image" src="https://github.com/user-attachments/assets/fffffc8a-1c97-4939-ad7c-08ad23b8ef94" />

<img width="1256" height="495" alt="image" src="https://github.com/user-attachments/assets/a0149c12-2fd0-4f95-a159-589bbf439b4e" />

<img width="1129" height="397" alt="image" src="https://github.com/user-attachments/assets/4206a096-840c-41d5-8c0b-e95c0170679c" />

<img width="1035" height="424" alt="image" src="https://github.com/user-attachments/assets/9ccf288f-0931-4bc6-be24-19dea51a7cc2" />

<img width="1109" height="389" alt="image" src="https://github.com/user-attachments/assets/afd08698-5aa9-4bc1-86c2-42ac162e1aaf" />

<img width="619" height="441" alt="image" src="https://github.com/user-attachments/assets/0328eded-37f1-4878-b3bf-5e340ce8fcc0" />

---

## Getting Started

### Prerequisites
- [.NET SDK 8.0+]
- [SQL Server]

---

### Installation / Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/yourusername/TransactionApp.git
   cd TransactionApp

2. **Restore Dependencies**
   ```bash
    dotnet restore

3. **Database Setup**
   
   *  Update your connection string
      *  Please double check the connection string By default in appsettings.json it will be point to sql local db (Server=(localdb)\MSSQLLocalDB;Database=NATO). This can be ammeded there or overriden in appsettings.Development.json
    
   *  Apply EF Core migrations:
      *  dotnet ef database update --project TransactionApp.Infrastructure --startup-project TransactionApp.Api
    
4. Run the project - Swagger UI will open up.

---
## Project Information

### Technologies Used
      *  .NET 8 Web API (ASP.NET Core)
      *  Entity Framework Core (with SQL Server provider)
      *  AutoMapper (DTO ↔ Entity mapping)
      *  FluentValidation (DTO validation)
      *  xUnit + Moq (testing)
      *  IMemoryCache (caching summaries)
      *  ILogger (structured logging)


### Road Map
  *  Database Indexing
      *  Introduce database indexes on frequently queried columns (e.g., UserId, TransactionType, CreatedAt) to improve read performance as data volume grows.
  *  Caching Strategy Improvements
      *  Migrate from in-memory caching to a distributed cache (e.g., Redis) for scalability in multi-instance deployments. If the database grows significantly, consider alternative data storage or sharding strategies to maintain performance.   
  *  Introduce MediatR Pattern
      *  Enables cleaner CQRS separation, reduces controller-service coupling, centralizes cross-cutting concerns (logging, validation, caching), and improves testability by isolating request handlers.  
  *  Filtering & Sorting
      
  

