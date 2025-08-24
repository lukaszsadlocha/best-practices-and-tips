using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IGetLowStockProductsUseCase
{
    Task<IEnumerable<LowStockProductDto>> ExecuteAsync(int reorderThreshold = 10, CancellationToken cancellationToken = default);
}
