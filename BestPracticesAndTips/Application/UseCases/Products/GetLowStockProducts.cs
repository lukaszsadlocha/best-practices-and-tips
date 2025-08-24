using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.UseCases.Products;

public class GetLowStockProductsUseCase(IProductRepository productRepository) : IGetLowStockProductsUseCase
{
    public async Task<IEnumerable<LowStockProductDto>> ExecuteAsync(int reorderThreshold = 10, CancellationToken cancellationToken = default)
    {
        var activeProducts = await productRepository.GetActiveProductsAsync();
        
        var lowStockProducts = activeProducts
            .Where(p => p.StockQuantity <= reorderThreshold)
            .OrderBy(p => p.StockQuantity)
            .ThenBy(p => p.Name)
            .Select(product => new LowStockProductDto
            {
                Id = product.Id,
                Name = product.Name,
                SKU = product.SKU,
                Category = product.Category,
                CurrentStock = product.StockQuantity,
                ReorderThreshold = reorderThreshold,
                ReorderQuantity = CalculateReorderQuantity(product.StockQuantity, reorderThreshold),
                Price = product.Price,
                DaysOutOfStock = product.StockQuantity == 0 ? CalculateDaysOutOfStock(product.UpdatedAt) : 0,
                IsOutOfStock = product.StockQuantity == 0
            })
            .ToList();

        return lowStockProducts;
    }

    private static int CalculateReorderQuantity(int currentStock, int reorderThreshold)
    {
        // Simple reorder quantity calculation: order enough to reach 2x threshold
        var targetStock = reorderThreshold * 2;
        return Math.Max(targetStock - currentStock, reorderThreshold);
    }

    private static int CalculateDaysOutOfStock(DateTime lastUpdated)
    {
        // Calculate days since last stock update (approximation for out-of-stock duration)
        return (DateTime.UtcNow - lastUpdated).Days;
    }
}
