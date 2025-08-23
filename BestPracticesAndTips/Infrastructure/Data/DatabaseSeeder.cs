using BestPracticesAndTips.Domain.Entities;
using BestPracticesAndTips.Domain.Enums;

namespace BestPracticesAndTips.Infrastructure.Data;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        if (context.Customers.Any())
            return; // Database has been seeded
        
        // Seed Customers
        var customers = new[]
        {
            new Customer
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@email.com",
                PhoneNumber = "+1234567890",
                DateOfBirth = new DateTime(1985, 5, 15),
                Address = "123 Main St",
                City = "New York",
                Country = "USA",
                PostalCode = "10001"
            },
            new Customer
            {
                FirstName = "Jane",
                LastName = "Smith",
                Email = "jane.smith@email.com",
                PhoneNumber = "+1234567891",
                DateOfBirth = new DateTime(1990, 8, 22),
                Address = "456 Oak Ave",
                City = "Los Angeles",
                Country = "USA",
                PostalCode = "90210"
            },
            new Customer
            {
                FirstName = "Bob",
                LastName = "Johnson",
                Email = "bob.johnson@email.com",
                PhoneNumber = "+1234567892",
                DateOfBirth = new DateTime(1978, 12, 3),
                Address = "789 Pine Rd",
                City = "Chicago",
                Country = "USA",
                PostalCode = "60601"
            }
        };
        
        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync();
        
        // Seed Products
        var products = new[]
        {
            new Product
            {
                Name = "Laptop Pro 15\"",
                Description = "High-performance laptop for professional work",
                Price = 1299.99m,
                Category = "Electronics",
                SKU = "LAP-001",
                StockQuantity = 50,
                ImageUrl = "https://example.com/laptop.jpg"
            },
            new Product
            {
                Name = "Wireless Mouse",
                Description = "Ergonomic wireless mouse with precision tracking",
                Price = 49.99m,
                Category = "Electronics",
                SKU = "MOU-001",
                StockQuantity = 200,
                ImageUrl = "https://example.com/mouse.jpg"
            },
            new Product
            {
                Name = "Coffee Maker",
                Description = "Premium coffee maker with programmable features",
                Price = 199.99m,
                Category = "Appliances",
                SKU = "COF-001",
                StockQuantity = 75,
                ImageUrl = "https://example.com/coffee.jpg"
            },
            new Product
            {
                Name = "Running Shoes",
                Description = "Comfortable running shoes for daily workouts",
                Price = 89.99m,
                Category = "Sports",
                SKU = "SHO-001",
                StockQuantity = 120,
                ImageUrl = "https://example.com/shoes.jpg"
            },
            new Product
            {
                Name = "Desk Chair",
                Description = "Ergonomic office chair with lumbar support",
                Price = 249.99m,
                Category = "Furniture",
                SKU = "CHA-001",
                StockQuantity = 30,
                ImageUrl = "https://example.com/chair.jpg"
            }
        };
        
        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();
        
        // Seed Orders
        var orders = new[]
        {
            new Order
            {
                CustomerId = customers[0].Id,
                OrderDate = DateTime.UtcNow.AddDays(-5),
                Status = OrderStatus.Delivered,
                ShippingAddress = customers[0].Address + ", " + customers[0].City,
                BillingAddress = customers[0].Address + ", " + customers[0].City,
                Notes = "First order from this customer"
            },
            new Order
            {
                CustomerId = customers[1].Id,
                OrderDate = DateTime.UtcNow.AddDays(-3),
                Status = OrderStatus.Processing,
                ShippingAddress = customers[1].Address + ", " + customers[1].City,
                BillingAddress = customers[1].Address + ", " + customers[1].City,
                Notes = "Rush delivery requested"
            },
            new Order
            {
                CustomerId = customers[2].Id,
                OrderDate = DateTime.UtcNow.AddDays(-1),
                Status = OrderStatus.Pending,
                ShippingAddress = customers[2].Address + ", " + customers[2].City,
                BillingAddress = customers[2].Address + ", " + customers[2].City
            }
        };
        
        await context.Orders.AddRangeAsync(orders);
        await context.SaveChangesAsync();
        
        // Seed Order Items
        var orderItems = new[]
        {
            // Order 1 items
            new OrderItem
            {
                OrderId = orders[0].Id,
                ProductId = products[0].Id,
                Quantity = 1,
                UnitPrice = products[0].Price
            },
            new OrderItem
            {
                OrderId = orders[0].Id,
                ProductId = products[1].Id,
                Quantity = 2,
                UnitPrice = products[1].Price
            },
            
            // Order 2 items
            new OrderItem
            {
                OrderId = orders[1].Id,
                ProductId = products[2].Id,
                Quantity = 1,
                UnitPrice = products[2].Price
            },
            new OrderItem
            {
                OrderId = orders[1].Id,
                ProductId = products[4].Id,
                Quantity = 1,
                UnitPrice = products[4].Price
            },
            
            // Order 3 items
            new OrderItem
            {
                OrderId = orders[2].Id,
                ProductId = products[3].Id,
                Quantity = 2,
                UnitPrice = products[3].Price
            }
        };
        
        await context.OrderItems.AddRangeAsync(orderItems);
        await context.SaveChangesAsync();
        
        // Update order totals
        foreach (var order in orders)
        {
            order.TotalAmount = order.CalculateTotalAmount();
        }
        
        await context.SaveChangesAsync();
    }
}
