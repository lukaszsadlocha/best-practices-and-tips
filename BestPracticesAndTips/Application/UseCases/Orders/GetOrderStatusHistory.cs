using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Enums;

namespace BestPracticesAndTips.Application.UseCases.Orders;

public class GetOrderStatusHistoryUseCase(IOrderRepository orderRepository) : IGetOrderStatusHistoryUseCase
{
    public async Task<OrderStatusHistoryDto?> ExecuteAsync(int orderId, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(orderId);
        if (order == null)
        {
            return null;
        }

        // Since we don't have a dedicated status history table, we'll reconstruct from available data
        var statusHistory = new List<StatusChangeDto>();

        // Order created (always first)
        statusHistory.Add(new StatusChangeDto
        {
            Status = OrderStatus.Pending,
            Timestamp = order.CreatedAt,
            StatusName = "Pending",
            DurationInStatus = 0
        });

        // Add shipped status if applicable
        if (order.Status == OrderStatus.Shipped || order.Status == OrderStatus.Completed)
        {
            var shippedDate = order.ShippedDate ?? order.UpdatedAt;
            var pendingDuration = (int)(shippedDate - order.CreatedAt).TotalHours;
            
            statusHistory[0] = statusHistory[0] with { DurationInStatus = pendingDuration };
            
            statusHistory.Add(new StatusChangeDto
            {
                Status = OrderStatus.Shipped,
                Timestamp = shippedDate,
                StatusName = "Shipped",
                DurationInStatus = 0
            });
        }

        // Add completed status if applicable
        if (order.Status == OrderStatus.Completed)
        {
            var shippedStatusChange = statusHistory.FirstOrDefault(s => s.Status == OrderStatus.Shipped);
            if (shippedStatusChange != null)
            {
                var shippedDuration = (int)(order.UpdatedAt - shippedStatusChange.Timestamp).TotalHours;
                var shippedIndex = statusHistory.FindIndex(s => s.Status == OrderStatus.Shipped);
                statusHistory[shippedIndex] = statusHistory[shippedIndex] with { DurationInStatus = shippedDuration };
            }

            statusHistory.Add(new StatusChangeDto
            {
                Status = OrderStatus.Completed,
                Timestamp = order.UpdatedAt,
                StatusName = "Completed",
                DurationInStatus = 0
            });
        }

        // Add cancelled status if applicable
        if (order.Status == OrderStatus.Cancelled)
        {
            var lastStatus = statusHistory.Last();
            var duration = (int)(order.UpdatedAt - lastStatus.Timestamp).TotalHours;
            var lastIndex = statusHistory.Count - 1;
            statusHistory[lastIndex] = statusHistory[lastIndex] with { DurationInStatus = duration };

            statusHistory.Add(new StatusChangeDto
            {
                Status = OrderStatus.Cancelled,
                Timestamp = order.UpdatedAt,
                StatusName = "Cancelled",
                DurationInStatus = 0
            });
        }

        // Calculate current duration for the last status
        if (statusHistory.Any() && statusHistory.Last().DurationInStatus == 0)
        {
            var lastStatus = statusHistory.Last();
            var currentDuration = (int)(DateTime.UtcNow - lastStatus.Timestamp).TotalHours;
            var lastIndex = statusHistory.Count - 1;
            statusHistory[lastIndex] = statusHistory[lastIndex] with { DurationInStatus = currentDuration };
        }

        return new OrderStatusHistoryDto
        {
            OrderId = order.Id,
            StatusHistory = statusHistory
        };
    }
}
