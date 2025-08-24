using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IGetOrderStatusHistoryUseCase
{
    Task<OrderStatusHistoryDto?> ExecuteAsync(int orderId, CancellationToken cancellationToken = default);
}
