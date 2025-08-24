using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface ICreateCustomerUseCase
{
    Task<CustomerDto> ExecuteAsync(CreateCustomerDto customer, CancellationToken cancellationToken = default);
}
