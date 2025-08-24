using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IGetCustomerByIdUseCase
{
    Task<CustomerDto?> ExecuteAsync(int id, CancellationToken cancellationToken = default);
}
