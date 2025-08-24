using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IGetProductByIdUseCase
{
    Task<ProductDto?> ExecuteAsync(int id, CancellationToken cancellationToken = default);
}
