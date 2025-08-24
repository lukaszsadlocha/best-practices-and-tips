using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface ICreateOrderUseCase
{
    Task<OrderDto> ExecuteAsync(CreateOrderDto order, CancellationToken cancellationToken = default);
}
