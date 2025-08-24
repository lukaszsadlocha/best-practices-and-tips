using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Enums;

namespace BestPracticesAndTips.Application.Common.Interfaces.UseCases;

public interface IGetOrdersUseCase
{
    Task<IEnumerable<OrderDto>> ExecuteAsync(int? customerId = null, OrderStatus? status = null, bool recentOnly = false, int recentDays = 30, CancellationToken cancellationToken = default);
}
