using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IUpdateProductUseCase
{
    Task<ProductDto> ExecuteAsync(int id, UpdateProductDto product, CancellationToken cancellationToken = default);
}
