using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IShipOrderUseCase
{
    Task<OrderDto> ExecuteAsync(int id, CancellationToken cancellationToken = default);
}
