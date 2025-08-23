using BestPracticesAndTips.Domain.Enums;

namespace BestPracticesAndTips.Application.DTOs;

public record OrderDto
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public string BillingAddress { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime? ShippedDate { get; set; }
    public IReadOnlyCollection<OrderItemDto> OrderItems { get; set; } = [];
}

public record OrderItemDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal SubTotal { get; set; }
}

public record CreateOrderDto
{
    public int CustomerId { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public string BillingAddress { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public IReadOnlyCollection<CreateOrderItemDto> OrderItems { get; set; } = [];
}

public record CreateOrderItemDto
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}
