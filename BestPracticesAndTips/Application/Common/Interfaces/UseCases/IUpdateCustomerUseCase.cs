using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IUpdateCustomerUseCase
{
    Task<CustomerDto> ExecuteAsync(int id, UpdateCustomerDto customer, CancellationToken cancellationToken = default);
}
