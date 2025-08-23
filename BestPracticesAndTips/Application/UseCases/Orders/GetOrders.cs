using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;
using BestPracticesAndTips.Domain.Enums;
using MediatR;

namespace BestPracticesAndTips.Application.UseCases.Orders;

public class GetOrdersQuery : IRequest<IEnumerable<OrderDto>>
{
    public int? CustomerId { get; init; }
    public OrderStatus? Status { get; init; }
    public bool RecentOnly { get; init; }
    public int RecentDays { get; init; } = 30;
}

public class GetOrdersQueryHandler(IOrderRepository orderRepository)
    : IRequestHandler<GetOrdersQuery, IEnumerable<OrderDto>>
{
    public async Task<IEnumerable<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Order> orders;

        if (request.CustomerId.HasValue)
        {
            orders = await orderRepository.GetOrdersByCustomerIdAsync(request.CustomerId.Value);
        }
        else if (request.Status.HasValue)
        {
            orders = await orderRepository.GetOrdersByStatusAsync(request.Status.Value);
        }
        else if (request.RecentOnly)
        {
            orders = await orderRepository.GetRecentOrdersAsync(request.RecentDays);
        }
        else
        {
            orders = await orderRepository.GetAllAsync();
        }

        return orders.Select(MapToDto);
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
