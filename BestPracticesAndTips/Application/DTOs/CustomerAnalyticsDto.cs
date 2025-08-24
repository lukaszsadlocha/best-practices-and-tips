namespace BestPracticesAndTips.Application.DTOs;

public record CustomerAnalyticsDto
{
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int TotalOrders { get; set; }
    public decimal TotalSpent { get; set; }
    public decimal AverageOrderValue { get; set; }
    public DateTime? LastOrderDate { get; set; }
    public IReadOnlyCollection<FavoriteProductDto> FavoriteProducts { get; set; } = [];
    public OrderSummaryDto OrderSummary { get; set; } = new();
}

public record FavoriteProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int TotalQuantityOrdered { get; set; }
    public decimal TotalSpentOnProduct { get; set; }
}

public record OrderSummaryDto
{
    public int PendingOrders { get; set; }
    public int ShippedOrders { get; set; }
    public int CompletedOrders { get; set; }
    public int CancelledOrders { get; set; }
}
