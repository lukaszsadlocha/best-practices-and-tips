using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface ICancelOrderUseCase
{
    Task<OrderDto> ExecuteAsync(int id, CancellationToken cancellationToken = default);
}
