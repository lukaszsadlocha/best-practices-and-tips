namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IDeleteCustomerUseCase
{
    Task<bool> ExecuteAsync(int id, CancellationToken cancellationToken = default);
}
