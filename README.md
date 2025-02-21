# Book Management API - Structure Overview  

## 📌 Project Structure  

The API follows a **three-layered architecture**:  

## 📂 Folder Breakdown  

### 1️⃣ **Controllers/** (Handles API Requests)  
Contains controllers that define API endpoints. Example:  
- **`BooksController.cs`** → Manages book operations (GET, POST, PUT, DELETE).  
- **`AuthController.cs`** → Handles authentication and JWT token generation.  

### 2️⃣ **Models/** (Defines Data Structures)  
Contains C# classes that represent the database tables. Example:  
- **`Book.cs`** → Defines properties like `Title`, `AuthorName`, `PublicationYear`, etc.  

### 3️⃣ **Data/** (Database Configuration)  
Manages the connection to SQL Server using **Entity Framework Core (EF Core)**.  
- **`BookDbContext.cs`** → Configures the `Books` table.  
- **Migrations/** → Stores EF Core migrations to keep track of schema changes.  

### 4️⃣ **Repositories/** (Data Access Layer)  
Handles database queries and business logic using **Dependency Injection**. Example:  
- **`IBookRepository.cs`** → Defines interfaces for data access methods.  
- **`BookRepository.cs`** → Implements database logic (CRUD operations).  

### 5️⃣ **Configuration Files**  
- **`appsettings.json`** → Stores database connection strings and JWT settings.  
- **`Program.cs`** → Configures middleware, authentication, and services.  

## 🔥 API Functionality  

The API supports the following:  
✅ JWT Authentication (Login & Protected Endpoints)  
✅ CRUD Operations for Books (Create, Read, Update, Soft Delete)  
✅ Sorting & Pagination (Retrieve Books by Popularity)  
✅ Soft Deletion Instead of Permanent Removal  
✅ Swagger API Documentation  
