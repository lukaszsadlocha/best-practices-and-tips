using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Enums;

namespace BestPracticesAndTips.Application.UseCases.Customers;

public class GetCustomerAnalyticsUseCase(ICustomerRepository customerRepository, IOrderRepository orderRepository) : IGetCustomerAnalyticsUseCase
{
    public async Task<CustomerAnalyticsDto?> ExecuteAsync(int customerId, CancellationToken cancellationToken = default)
    {
        var customer = await customerRepository.GetByIdAsync(customerId);
        if (customer == null || !customer.IsActive)
        {
            return null;
        }

        var orders = await orderRepository.GetOrdersByCustomerIdAsync(customerId);
        var completedOrders = orders.Where(o => o.Status == OrderStatus.Completed).ToList();

        if (!orders.Any())
        {
            return new CustomerAnalyticsDto
            {
                CustomerId = customer.Id,
                CustomerName = customer.GetFullName(),
                TotalOrders = 0,
                TotalSpent = 0,
                AverageOrderValue = 0,
                LastOrderDate = null,
                FavoriteProducts = [],
                OrderSummary = new OrderSummaryDto()
            };
        }

        var totalSpent = completedOrders.Sum(o => o.TotalAmount);
        var totalOrders = orders.Count();
        var averageOrderValue = completedOrders.Any() ? totalSpent / completedOrders.Count : 0;

        // Calculate favorite products based on completed orders
        var favoriteProducts = completedOrders
            .SelectMany(o => o.OrderItems)
            .GroupBy(oi => new { oi.ProductId, oi.Product?.Name })
            .Select(g => new FavoriteProductDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name ?? "Unknown",
                TotalQuantityOrdered = g.Sum(oi => oi.Quantity),
                TotalSpentOnProduct = g.Sum(oi => oi.Quantity * oi.UnitPrice)
            })
            .OrderByDescending(fp => fp.TotalQuantityOrdered)
            .Take(5)
            .ToList();

        var orderSummary = new OrderSummaryDto
        {
            PendingOrders = orders.Count(o => o.Status == OrderStatus.Pending),
            ShippedOrders = orders.Count(o => o.Status == OrderStatus.Shipped),
            CompletedOrders = orders.Count(o => o.Status == OrderStatus.Completed),
            CancelledOrders = orders.Count(o => o.Status == OrderStatus.Cancelled)
        };

        return new CustomerAnalyticsDto
        {
            CustomerId = customer.Id,
            CustomerName = customer.GetFullName(),
            TotalOrders = totalOrders,
            TotalSpent = totalSpent,
            AverageOrderValue = averageOrderValue,
            LastOrderDate = orders.OrderByDescending(o => o.OrderDate).FirstOrDefault()?.OrderDate,
            FavoriteProducts = favoriteProducts,
            OrderSummary = orderSummary
        };
    }
}
