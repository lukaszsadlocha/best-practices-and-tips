using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;

namespace BestPracticesAndTips.Application.UseCases.Products;

public class GetProductCatalogUseCase(IProductRepository productRepository) : IGetProductCatalogUseCase
{
    public async Task<ProductCatalogDto> ExecuteAsync(ProductCatalogFilterDto filter, CancellationToken cancellationToken = default)
    {
        // Start with active products
        var products = await productRepository.GetActiveProductsAsync();

        // Apply filters
        if (!string.IsNullOrEmpty(filter.Category))
        {
            products = products.Where(p => p.Category.Equals(filter.Category, StringComparison.OrdinalIgnoreCase));
        }

        if (filter.MinPrice.HasValue)
        {
            products = products.Where(p => p.Price >= filter.MinPrice.Value);
        }

        if (filter.MaxPrice.HasValue)
        {
            products = products.Where(p => p.Price <= filter.MaxPrice.Value);
        }

        if (filter.InStockOnly)
        {
            products = products.Where(p => p.StockQuantity > 0);
        }

        if (!string.IsNullOrEmpty(filter.SearchTerm))
        {
            products = products.Where(p => 
                p.Name.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.Description.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                p.SKU.Contains(filter.SearchTerm, StringComparison.OrdinalIgnoreCase));
        }

        var productsList = products.ToList();
        var totalCount = productsList.Count;

        // Apply pagination
        var pagedProducts = productsList
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .Select(MapToDto)
            .ToList();

        var totalPages = (int)Math.Ceiling((double)totalCount / filter.PageSize);

        return new ProductCatalogDto
        {
            Products = pagedProducts,
            Page = filter.Page,
            PageSize = filter.PageSize,
            TotalCount = totalCount,
            TotalPages = totalPages,
            HasNextPage = filter.Page < totalPages,
            HasPreviousPage = filter.Page > 1
        };
    }

    private static ProductDto MapToDto(Product product)
    {
        return new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Category = product.Category,
            SKU = product.SKU,
            StockQuantity = product.StockQuantity,
            IsActive = product.IsActive,
            ImageUrl = product.ImageUrl,
            IsInStock = product.IsInStock()
        };
    }
}
