using BestPracticesAndTips.Domain.Entities;

namespace BestPracticesAndTips.Application.Common.Interfaces;

public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByEmailAsync(string email);
    Task<IEnumerable<Customer>> GetActiveCustomersAsync();
    Task<IEnumerable<Customer>> GetCustomersWithOrdersAsync();
}
