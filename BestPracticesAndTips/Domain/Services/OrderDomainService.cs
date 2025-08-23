using BestPracticesAndTips.Domain.Entities;
using BestPracticesAndTips.Domain.Enums;

namespace BestPracticesAndTips.Domain.Services;

public class OrderDomainService : IOrderDomainService
{
    public async Task<Order> CreateOrderAsync(Customer customer, List<OrderItem> orderItems, 
        string shippingAddress, string billingAddress, string? notes = null)
    {
        var order = new Order
        {
            CustomerId = customer.Id,
            Customer = customer,
            OrderDate = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            ShippingAddress = shippingAddress,
            BillingAddress = billingAddress,
            Notes = notes,
            OrderItems = orderItems
        };

        // Calculate total amount
        order.TotalAmount = order.CalculateTotalAmount();

        return await Task.FromResult(order);
    }

    public async Task<bool> CanFulfillOrderAsync(List<OrderItem> orderItems)
    {
        // Domain logic to check if we can fulfill the order
        // In a real scenario, this would check inventory, product availability, etc.
        foreach (var item in orderItems)
        {
            if (item.Product == null || !item.Product.IsActive || !item.Product.IsInStock())
            {
                return false;
            }

            if (item.Product.StockQuantity < item.Quantity)
            {
                return false;
            }
        }

        return await Task.FromResult(true);
    }

    public async Task ProcessOrderAsync(Order order)
    {
        // Domain logic for processing an order
        if (order.Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException("Only pending orders can be processed.");
        }

        // Update product stock quantities
        foreach (var item in order.OrderItems)
        {
            if (item.Product != null)
            {
                item.Product.UpdateStock(-item.Quantity);
            }
        }

        order.Status = OrderStatus.Processing;
        order.UpdatedAt = DateTime.UtcNow;

        await Task.CompletedTask;
    }
}
