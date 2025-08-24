using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;
using BestPracticesAndTips.Domain.Enums;

namespace BestPracticesAndTips.Application.UseCases.Orders;

public class CompleteOrderUseCase(IOrderRepository orderRepository) : ICompleteOrderUseCase
{
    public async Task<OrderDto> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(id);
        if (order == null)
        {
            throw new InvalidOperationException($"Order with ID {id} not found.");
        }

        // Business rule: can only complete shipped orders
        if (order.Status != OrderStatus.Shipped)
        {
            throw new InvalidOperationException($"Cannot complete order with status '{order.Status}'. Only shipped orders can be completed.");
        }

        // Complete order using domain method
        order.CompleteOrder();
        var completedOrder = await orderRepository.UpdateAsync(order);

        return MapToDto(completedOrder);
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
