using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface ICreateProductUseCase
{
    Task<ProductDto> ExecuteAsync(CreateProductDto product, CancellationToken cancellationToken = default);
}
