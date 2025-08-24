using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Presentation.Endpoints;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/products")
            .WithTags("Products")
            .WithOpenApi();

        group.MapGet("/", async (IGetProductsUseCase useCase, bool activeOnly = true, string? category = null) =>
        {
            var products = await useCase.ExecuteAsync(activeOnly, category);
            return Results.Ok(products);
        })
        .WithName("GetProducts")
        .WithSummary("Get all products")
        .Produces<IEnumerable<ProductDto>>();

        group.MapGet("/{id:int}", async (IGetProductByIdUseCase useCase, int id) =>
        {
            var product = await useCase.ExecuteAsync(id);
            return product is not null ? Results.Ok(product) : Results.NotFound();
        })
        .WithName("GetProductById")
        .WithSummary("Get product by ID")
        .Produces<ProductDto>()
        .Produces(404);

        group.MapPost("/", async (ICreateProductUseCase useCase, CreateProductDto productDto) =>
        {
            try
            {
                var product = await useCase.ExecuteAsync(productDto);
                return Results.Created($"/api/products/{product.Id}", product);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CreateProduct")
        .WithSummary("Create a new product")
        .Produces<ProductDto>(201)
        .Produces(400);

        group.MapPut("/{id:int}", async (IUpdateProductUseCase useCase, int id, UpdateProductDto productDto) =>
        {
            try
            {
                var product = await useCase.ExecuteAsync(id, productDto);
                return Results.Ok(product);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("UpdateProduct")
        .WithSummary("Update product details")
        .Produces<ProductDto>()
        .Produces(400)
        .Produces(404);

        group.MapDelete("/{id:int}", async (IDeleteProductUseCase useCase, int id) =>
        {
            try
            {
                var result = await useCase.ExecuteAsync(id);
                return result ? Results.NoContent() : Results.NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("DeleteProduct")
        .WithSummary("Delete (deactivate) product")
        .Produces(204)
        .Produces(400)
        .Produces(404);

        group.MapPatch("/{id:int}/stock", async (IUpdateStockUseCase useCase, int id, int quantity) =>
        {
            try
            {
                var product = await useCase.ExecuteAsync(id, quantity);
                return Results.Ok(product);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("UpdateStock")
        .WithSummary("Adjust product stock (positive to increase, negative to decrease)")
        .Produces<ProductDto>()
        .Produces(400)
        .Produces(404);

        group.MapGet("/catalog", async (IGetProductCatalogUseCase useCase, 
            int page = 1, int pageSize = 10, string? category = null, 
            decimal? minPrice = null, decimal? maxPrice = null, 
            bool inStockOnly = true, string? searchTerm = null) =>
        {
            var filter = new ProductCatalogFilterDto
            {
                Page = page,
                PageSize = pageSize,
                Category = category,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                InStockOnly = inStockOnly,
                SearchTerm = searchTerm
            };

            var catalog = await useCase.ExecuteAsync(filter);
            return Results.Ok(catalog);
        })
        .WithName("GetProductCatalog")
        .WithSummary("Get paginated product catalog with filtering")
        .Produces<ProductCatalogDto>();

        group.MapGet("/low-stock", async (IGetLowStockProductsUseCase useCase, int threshold = 10) =>
        {
            var lowStockProducts = await useCase.ExecuteAsync(threshold);
            return Results.Ok(lowStockProducts);
        })
        .WithName("GetLowStockProducts")
        .WithSummary("Get products below reorder threshold")
        .Produces<IEnumerable<LowStockProductDto>>();
    }
}
