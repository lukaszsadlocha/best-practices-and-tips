using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IUpdateOrderUseCase
{
    Task<OrderDto> ExecuteAsync(int id, UpdateOrderDto order, CancellationToken cancellationToken = default);
}
