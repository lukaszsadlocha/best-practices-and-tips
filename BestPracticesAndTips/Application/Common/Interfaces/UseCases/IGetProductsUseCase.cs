using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IGetProductsUseCase
{
    Task<IEnumerable<ProductDto>> ExecuteAsync(bool activeOnly = true, string? category = null, CancellationToken cancellationToken = default);
}
