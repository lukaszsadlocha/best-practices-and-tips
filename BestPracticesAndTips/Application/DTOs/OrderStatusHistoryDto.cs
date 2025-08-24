using BestPracticesAndTips.Domain.Enums;

namespace BestPracticesAndTips.Application.DTOs;

public record OrderStatusHistoryDto
{
    public int OrderId { get; set; }
    public IReadOnlyCollection<StatusChangeDto> StatusHistory { get; set; } = [];
}

public record StatusChangeDto
{
    public OrderStatus Status { get; set; }
    public DateTime Timestamp { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public int DurationInStatus { get; set; } // Duration in hours
}
