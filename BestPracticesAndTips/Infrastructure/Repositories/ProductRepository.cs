using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Domain.Entities;
using BestPracticesAndTips.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BestPracticesAndTips.Infrastructure.Repositories;

public class ProductRepository : Repository<Product>, IProductRepository
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetActiveProductsAsync()
    {
        return await _dbSet.Where(p => p.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(string category)
    {
        return await _dbSet
            .Where(p => p.Category == category && p.IsActive)
            .ToListAsync();
    }

    public async Task<Product?> GetBySkuAsync(string sku)
    {
        return await _dbSet.FirstOrDefaultAsync(p => p.SKU == sku);
    }

    public async Task<IEnumerable<Product>> GetLowStockProductsAsync(int threshold = 10)
    {
        return await _dbSet
            .Where(p => p.StockQuantity <= threshold && p.IsActive)
            .ToListAsync();
    }
}
