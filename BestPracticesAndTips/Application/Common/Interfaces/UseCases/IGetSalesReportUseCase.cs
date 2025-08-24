using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IGetSalesReportUseCase
{
    Task<SalesReportDto> ExecuteAsync(SalesReportFilterDto filter, CancellationToken cancellationToken = default);
}
