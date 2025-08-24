using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;

namespace BestPracticesAndTips.Application.UseCases.Products;

public class CreateProductUseCase(IProductRepository productRepository) : ICreateProductUseCase
{
    public async Task<ProductDto> ExecuteAsync(CreateProductDto productDto, CancellationToken cancellationToken = default)
    {
        // Validate unique SKU
        if (!string.IsNullOrEmpty(productDto.SKU))
        {
            var existingProduct = await productRepository.GetBySkuAsync(productDto.SKU);
            if (existingProduct != null)
            {
                throw new InvalidOperationException($"Product with SKU '{productDto.SKU}' already exists.");
            }
        }

        // Validate price
        if (productDto.Price <= 0)
        {
            throw new ArgumentException("Product price must be greater than zero.");
        }

        // Validate stock quantity
        if (productDto.StockQuantity < 0)
        {
            throw new ArgumentException("Stock quantity cannot be negative.");
        }

        var product = new Product
        {
            Name = productDto.Name,
            Description = productDto.Description,
            Price = productDto.Price,
            Category = productDto.Category,
            SKU = !string.IsNullOrEmpty(productDto.SKU) ? productDto.SKU : GenerateSKU(productDto.Name),
            StockQuantity = productDto.StockQuantity,
            ImageUrl = productDto.ImageUrl
        };

        var createdProduct = await productRepository.AddAsync(product);

        return MapToDto(createdProduct);
    }

    private static string GenerateSKU(string productName)
    {
        // Simple SKU generation logic
        var prefix = productName.Length >= 3 ? productName[..3].ToUpper() : productName.ToUpper();
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmm");
        return $"{prefix}-{timestamp}";
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
