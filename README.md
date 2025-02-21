# Book Management API

This is a RESTful API built with ASP.NET Core Web API designed for managing books. It supports CRUD operations, including adding single or multiple books, updating, soft deleting, and retrieving book details and lists with pagination. The API uses SQL Server for data storage with Entity Framework Core for data access, JWT-based authentication for security, and Swagger for API documentation.

## Features

- **CRUD Operations:**  
  - Add single and bulk books.
  - Update book details.
  - Soft delete single and multiple books.
  - Retrieve a list of books (titles only) sorted by a dynamically calculated popularity score.
  - Retrieve complete details of a specific book (including view count and dynamic popularity score).

- **Security:**  
  - JWT-based authentication to secure all endpoints.

- **Documentation:**  
  - Swagger UI for interactive API documentation.

- **Database:**  
  - SQL Server is used as the back-end database with EF Core for data access.

## Technologies Used

- ASP.NET Core Web API (.NET 8)
- C#
- SQL Server
- Entity Framework Core
- JWT Authentication
- Swagger
