using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IUpdateStockUseCase
{
    Task<ProductDto> ExecuteAsync(int id, int quantity, CancellationToken cancellationToken = default);
}
