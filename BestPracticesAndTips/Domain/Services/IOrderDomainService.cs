using BestPracticesAndTips.Domain.Entities;

namespace BestPracticesAndTips.Domain.Services;

public interface IOrderDomainService
{
    Task<Order> CreateOrderAsync(Customer customer, List<OrderItem> orderItems, string shippingAddress, string billingAddress, string? notes = null);
    Task<bool> CanFulfillOrderAsync(List<OrderItem> orderItems);
    Task ProcessOrderAsync(Order order);
}
