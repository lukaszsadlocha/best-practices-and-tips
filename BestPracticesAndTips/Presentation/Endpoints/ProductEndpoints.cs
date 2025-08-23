using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Application.UseCases.Products;
using MediatR;

namespace BestPracticesAndTips.Presentation.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products")
            .WithOpenApi();

        group.MapGet("/", async (IMediator mediator, bool activeOnly = true, string? category = null) =>
        {
            var query = new GetProductsQuery 
            { 
                ActiveOnly = activeOnly,
                Category = category
            };
            var products = await mediator.Send(query);
            return Results.Ok(products);
        })
        .WithName("GetProducts")
        .WithSummary("Get all products")
        .Produces<IEnumerable<ProductDto>>();

        group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
        {
            var query = new GetProductByIdQuery { Id = id };
            var product = await mediator.Send(query);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProductById")
        .WithSummary("Get product by ID")
        .Produces<ProductDto>()
        .Produces(404);
    }
}
