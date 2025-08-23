using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Domain.Entities;
using BestPracticesAndTips.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BestPracticesAndTips.Infrastructure.Repositories;

public class CustomerRepository(ApplicationDbContext context) : Repository<Customer>(context), ICustomerRepository
{
    public async Task<Customer?> GetByEmailAsync(string email)
    {
        return await _dbSet.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<IEnumerable<Customer>> GetActiveCustomersAsync()
    {
        return await _dbSet.Where(c => c.IsActive).ToListAsync();
    }

    public async Task<IEnumerable<Customer>> GetCustomersWithOrdersAsync()
    {
        return await _dbSet
            .Include(c => c.Orders)
            .Where(c => c.Orders.Any())
            .ToListAsync();
    }
}
