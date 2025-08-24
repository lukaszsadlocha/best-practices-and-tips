using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;

namespace BestPracticesAndTips.Application.UseCases.Products;

public class UpdateStockUseCase(IProductRepository productRepository) : IUpdateStockUseCase
{
    public async Task<ProductDto> ExecuteAsync(int id, int quantity, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with ID {id} not found.");
        }

        if (!product.IsActive)
        {
            throw new InvalidOperationException("Cannot update stock for inactive product.");
        }

        // Validate that stock won't go negative
        if (product.StockQuantity + quantity < 0)
        {
            throw new InvalidOperationException($"Cannot reduce stock by {Math.Abs(quantity)}. Current stock is {product.StockQuantity}.");
        }

        // Use domain method to update stock
        product.UpdateStock(quantity);
        
        var updatedProduct = await productRepository.UpdateAsync(product);

        return MapToDto(updatedProduct);
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
