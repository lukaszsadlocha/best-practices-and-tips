using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;

namespace BestPracticesAndTips.Application.UseCases.Products;

public class GetProductsUseCase(IProductRepository productRepository) : IGetProductsUseCase
{
    public async Task<IEnumerable<ProductDto>> ExecuteAsync(bool activeOnly = true, string? category = null, CancellationToken cancellationToken = default)
    {
        IEnumerable<Product> products;

        if (!string.IsNullOrEmpty(category))
        {
            products = await productRepository.GetProductsByCategoryAsync(category);
        }
        else if (activeOnly)
        {
            products = await productRepository.GetActiveProductsAsync();
        }
        else
        {
            products = await productRepository.GetAllAsync();
        }

        return products.Select(MapToDto);
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
