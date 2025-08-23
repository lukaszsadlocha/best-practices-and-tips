using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;
using MediatR;

namespace BestPracticesAndTips.Application.UseCases.Products;

public class GetProductsQuery : IRequest<IEnumerable<ProductDto>>
{
    public bool ActiveOnly { get; init; } = true;
    public string? Category { get; init; }
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, IEnumerable<ProductDto>>
{
    private readonly IProductRepository _productRepository;

    public GetProductsQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Product> products;

        if (!string.IsNullOrEmpty(request.Category))
        {
            products = await _productRepository.GetProductsByCategoryAsync(request.Category);
        }
        else if (request.ActiveOnly)
        {
            products = await _productRepository.GetActiveProductsAsync();
        }
        else
        {
            products = await _productRepository.GetAllAsync();
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
