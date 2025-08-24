namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IDeleteProductUseCase
{
    Task<bool> ExecuteAsync(int id, CancellationToken cancellationToken = default);
}
