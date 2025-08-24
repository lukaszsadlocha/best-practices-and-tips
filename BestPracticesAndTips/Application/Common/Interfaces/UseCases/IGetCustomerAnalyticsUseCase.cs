using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IGetCustomerAnalyticsUseCase
{
    Task<CustomerAnalyticsDto?> ExecuteAsync(int customerId, CancellationToken cancellationToken = default);
}
