using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IGetCustomersUseCase
{
    Task<IEnumerable<CustomerDto>> ExecuteAsync(bool activeOnly = true, CancellationToken cancellationToken = default);
}
