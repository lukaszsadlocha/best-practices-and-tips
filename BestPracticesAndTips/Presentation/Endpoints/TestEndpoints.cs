using BestPracticesAndTips.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BestPracticesAndTips.Presentation.Endpoints;

public static class TestEndpoints
{
    public static void MapTestEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/test")
            .WithTags("Test")
            .WithOpenApi();

        group.MapGet("/database-status", async (ApplicationDbContext context) =>
        {
            var customerCount = await context.Customers.CountAsync();
            var productCount = await context.Products.CountAsync();
            var orderCount = await context.Orders.CountAsync();
            var orderItemCount = await context.OrderItems.CountAsync();

            return Results.Ok(new
            {
                Status = "Connected",
                DatabaseType = "In-Memory",
                Statistics = new
                {
                    Customers = customerCount,
                    Products = productCount,
                    Orders = orderCount,
                    OrderItems = orderItemCount
                },
                Message = "Clean Architecture implementation with EF Core In-Memory database is working!"
            });
        })
        .WithName("GetDatabaseStatus")
        .WithSummary("Get database connection status and statistics");

        group.MapGet("/sample-data", async (ApplicationDbContext context) =>
        {
            var customers = await context.Customers.Take(3).ToListAsync();
            var products = await context.Products.Take(5).ToListAsync();
            var orders = await context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .Take(3)
                .ToListAsync();

            return Results.Ok(new
            {
                Customers = customers.Select(c => new
                {
                    c.Id,
                    FullName = c.GetFullName(),
                    c.Email,
                    c.City,
                    c.IsActive
                }),
                Products = products.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Price,
                    p.Category,
                    p.StockQuantity,
                    InStock = p.IsInStock()
                }),
                Orders = orders.Select(o => new
                {
                    o.Id,
                    CustomerName = o.Customer?.GetFullName(),
                    o.OrderDate,
                    Status = o.Status.ToString(),
                    o.TotalAmount,
                    ItemCount = o.OrderItems.Count
                })
            });
        })
        .WithName("GetSampleData")
        .WithSummary("Get sample data from all entities");
    }
}
