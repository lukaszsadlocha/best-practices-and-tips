using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface ICompleteOrderUseCase
{
    Task<OrderDto> ExecuteAsync(int id, CancellationToken cancellationToken = default);
}
