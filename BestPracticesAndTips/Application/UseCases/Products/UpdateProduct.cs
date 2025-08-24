using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;

namespace BestPracticesAndTips.Application.UseCases.Products;

public class UpdateProductUseCase(IProductRepository productRepository, IOrderRepository orderRepository) : IUpdateProductUseCase
{
    public async Task<ProductDto> ExecuteAsync(int id, UpdateProductDto productDto, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with ID {id} not found.");
        }

        if (!product.IsActive)
        {
            throw new InvalidOperationException("Cannot update inactive product.");
        }

        // Validate price
        if (productDto.Price <= 0)
        {
            throw new ArgumentException("Product price must be greater than zero.");
        }

        // Check if reducing stock below reserved quantities
        if (productDto.StockQuantity < product.StockQuantity)
        {
            var reservedQuantity = await CalculateReservedQuantityAsync(product.Id);
            if (productDto.StockQuantity < reservedQuantity)
            {
                throw new InvalidOperationException($"Cannot reduce stock below reserved quantity ({reservedQuantity} units).");
            }
        }

        // Update product properties
        product.Name = productDto.Name;
        product.Description = productDto.Description;
        product.Price = productDto.Price;
        product.Category = productDto.Category;
        product.StockQuantity = productDto.StockQuantity;
        product.ImageUrl = productDto.ImageUrl;
        product.UpdatedAt = DateTime.UtcNow;

        var updatedProduct = await productRepository.UpdateAsync(product);

        return MapToDto(updatedProduct);
    }

    private async Task<int> CalculateReservedQuantityAsync(int productId)
    {
        // Get all pending orders and calculate reserved quantity
        var pendingOrders = await orderRepository.GetOrdersByStatusAsync(Domain.Enums.OrderStatus.Pending);
        var reservedQuantity = pendingOrders
            .SelectMany(o => o.OrderItems)
            .Where(oi => oi.ProductId == productId)
            .Sum(oi => oi.Quantity);

        return reservedQuantity;
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
