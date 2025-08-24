using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Domain.Enums;

namespace BestPracticesAndTips.Application.UseCases.Customers;

public class DeleteCustomerUseCase(ICustomerRepository customerRepository, IOrderRepository orderRepository) : IDeleteCustomerUseCase
{
    public async Task<bool> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetByIdAsync(id);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {id} not found.");
        }

        if (!customer.IsActive)
        {
            throw new InvalidOperationException("Customer is already inactive.");
        }

        // Check for active orders - cannot delete customers with pending/shipped orders
        var activeOrders = await orderRepository.GetOrdersByCustomerIdAsync(customer.Id);
        var hasActiveOrders = activeOrders.Any(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.Shipped);
        
        if (hasActiveOrders)
        {
            throw new InvalidOperationException("Cannot delete customer with pending or shipped orders. Please complete or cancel all active orders first.");
        }

        // Soft delete using domain method
        customer.Deactivate();
        await customerRepository.UpdateAsync(customer);

        return true;
    }
}
