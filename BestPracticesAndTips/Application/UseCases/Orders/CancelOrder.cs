using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;
using BestPracticesAndTips.Domain.Enums;

namespace BestPracticesAndTips.Application.UseCases.Orders;

public class CancelOrderUseCase(IOrderRepository orderRepository, IProductRepository productRepository) : ICancelOrderUseCase
{
    public async Task<OrderDto> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(id);
        if (order == null)
        {
            throw new InvalidOperationException($"Order with ID {id} not found.");
        }

        // Business rule: can only cancel pending orders
        if (order.Status != OrderStatus.Pending)
        {
            throw new InvalidOperationException($"Cannot cancel order with status '{order.Status}'. Only pending orders can be cancelled.");
        }

        // Restore inventory for cancelled order items
        foreach (var orderItem in order.OrderItems)
        {
            var product = await productRepository.GetByIdAsync(orderItem.ProductId);
            if (product != null)
            {
                product.UpdateStock(orderItem.Quantity); // Add back to stock
                await productRepository.UpdateAsync(product);
            }
        }

        // Cancel order using domain method
        order.CancelOrder();
        var cancelledOrder = await orderRepository.UpdateAsync(order);

        return MapToDto(cancelledOrder);
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer?.GetFullName() ?? "Unknown",
            OrderDate = order.OrderDate,
            Status = order.Status.ToString(),
            TotalAmount = order.TotalAmount,
            ShippingAddress = order.ShippingAddress,
            BillingAddress = order.BillingAddress,
            Notes = order.Notes,
            ShippedDate = order.ShippedDate,
            OrderItems = order.OrderItems?.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                ProductName = oi.Product?.Name ?? "Unknown",
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                SubTotal = oi.GetSubTotal()
            }).ToList() ?? []
        };
    }
}
