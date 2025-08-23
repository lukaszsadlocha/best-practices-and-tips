# Best Practices and Tips - Clean Architecture Web API

This project demonstrates a comprehensive implementation of **Clean Architecture**, **Domain-Driven Design (DDD)**, and **Clean Code** principles using .NET 8, Entity Framework Core with In-Memory database, and Minimal API.

## 🏗️ Architecture Overview

The solution follows Clean Architecture principles with clear separation of concerns across four main layers:

```
┌─────────────────────────────────────────────────┐
│                 Presentation                    │
│              (Minimal API Endpoints)            │
├─────────────────────────────────────────────────┤
│                 Application                     │
│        (Use Cases, DTOs, Interfaces)           │
├─────────────────────────────────────────────────┤
│                Infrastructure                   │
│    (EF Core, Repositories, Data Access)        │
├─────────────────────────────────────────────────┤
│                   Domain                        │
│        (Entities, Value Objects, Rules)        │
└─────────────────────────────────────────────────┘
```

## 📁 Project Structure

### Domain Layer (`/Domain`)
- **Entities**: Core business entities (`Customer`, `Order`, `Product`, `OrderItem`)
- **Enums**: Business enumerations (`OrderStatus`)
- **Services**: Domain services containing business logic
- **Common**: Base classes and shared domain concepts

### Application Layer (`/Application`)
- **UseCases**: CQRS commands and queries using MediatR
- **DTOs**: Data Transfer Objects for API responses
- **Interfaces**: Repository and service contracts
- **Common**: Shared application models and responses

### Infrastructure Layer (`/Infrastructure`)
- **Data**: EF Core DbContext and configurations
- **Repositories**: Data access implementations
- **Seeder**: Database seeding logic

### Presentation Layer (`/Presentation`)
- **Endpoints**: Minimal API endpoint definitions organized by feature

## 🛠️ Technologies Used

- **.NET 8** - Latest .NET framework
- **Entity Framework Core 8** - ORM with In-Memory database
- **MediatR** - CQRS and mediator pattern implementation
- **FluentValidation** - Input validation
- **Minimal API** - Modern API development approach
- **Swagger/OpenAPI** - API documentation

## 🏛️ Domain Models

### Customer Entity (10 Properties)
- Id, FirstName, LastName, Email, PhoneNumber
- DateOfBirth, Address, City, Country, PostalCode
- IsActive (plus CreatedAt, UpdatedAt from BaseEntity)

### Order Entity (8 Properties)
- Id, CustomerId, OrderDate, Status, TotalAmount
- ShippingAddress, BillingAddress, Notes, ShippedDate
- (plus CreatedAt, UpdatedAt from BaseEntity)

### Product Entity (8 Properties)
- Id, Name, Description, Price, Category
- SKU, StockQuantity, IsActive, ImageUrl
- (plus CreatedAt, UpdatedAt from BaseEntity)

## 🔗 Entity Relationships

- **Customer** → **Order** (One-to-Many)
- **Order** → **OrderItem** (One-to-Many)
- **Product** → **OrderItem** (One-to-Many)

## 🚀 API Endpoints

### Customers
- `GET /api/customers` - Get all customers
- `GET /api/customers/{id}` - Get customer by ID
- `POST /api/customers` - Create new customer

### Products
- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID

### Orders
- `GET /api/orders` - Get all orders
- `GET /api/orders/customer/{customerId}` - Get orders by customer
- `GET /api/orders/status/{status}` - Get orders by status

### Test/Debug
- `GET /api/test/database-status` - Database connection and statistics
- `GET /api/test/sample-data` - Sample data from all entities

## 🎯 Design Patterns Implemented

1. **Repository Pattern** - Data access abstraction
2. **CQRS (Command Query Responsibility Segregation)** - Using MediatR
3. **Domain-Driven Design** - Rich domain models with business logic
4. **Dependency Injection** - Loose coupling and testability
5. **Factory Pattern** - In domain services for complex object creation

## 💾 Database Seeding

The application automatically seeds the in-memory database with:
- 3 Sample customers
- 5 Sample products across different categories
- 3 Sample orders with order items
- Realistic relationships and calculated totals

## 🏃 Running the Application

1. **Clone and restore packages**:
```bash
dotnet restore
```

2. **Run the application**:
```bash
dotnet run
```

3. **Access Swagger UI**:
   - Navigate to `https://localhost:[port]/swagger`
   - Test the endpoints interactively

4. **Test the setup**:
   - Visit `/api/test/database-status` to verify everything is working
   - Use `/api/test/sample-data` to see seeded data

## 🧪 Key Features Demonstrated

### Clean Code Principles
- **Single Responsibility Principle** - Each class has one reason to change
- **Open/Closed Principle** - Extensions without modifications
- **Dependency Inversion** - Abstractions over concretions
- **Clear naming** - Self-documenting code
- **Small methods** - Focused functionality

### Clean Architecture Benefits
- **Independence** - Business logic independent of frameworks
- **Testability** - Easy to unit test business rules
- **Database Independence** - Can switch from In-Memory to SQL Server easily
- **Framework Independence** - Core logic doesn't depend on ASP.NET Core

### Domain-Driven Design
- **Rich Domain Models** - Business logic in entities
- **Domain Services** - Complex business operations
- **Value Objects** - Concepts without identity
- **Repository Contracts** - Domain-defined data access needs

## 🔧 Extending the Application

This foundation can be easily extended with:

1. **Authentication & Authorization**
2. **Validation with FluentValidation**
3. **Logging with Serilog**
4. **Error Handling Middleware**
5. **AutoMapper for DTO mappings**
6. **Unit Tests with xUnit**
7. **Integration Tests**
8. **Real database (SQL Server, PostgreSQL)**
9. **API Versioning**
10. **Rate Limiting**

## 📚 Learning Resources

This implementation demonstrates best practices from:
- **Clean Architecture** by Robert C. Martin
- **Domain-Driven Design** by Eric Evans
- **Clean Code** by Robert C. Martin
- **.NET Documentation** and community best practices

---

The application is ready to run and demonstrates a production-ready structure that can scale and maintain high code quality standards! 🚀
