using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IGetProductCatalogUseCase
{
    Task<ProductCatalogDto> ExecuteAsync(ProductCatalogFilterDto filter, CancellationToken cancellationToken = default);
}
