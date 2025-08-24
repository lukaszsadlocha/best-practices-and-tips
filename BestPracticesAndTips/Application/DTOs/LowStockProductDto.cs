namespace BestPracticesAndTips.Application.DTOs;

public record LowStockProductDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string SKU { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int CurrentStock { get; set; }
    public int ReorderThreshold { get; set; }
    public int ReorderQuantity { get; set; }
    public decimal Price { get; set; }
    public int DaysOutOfStock { get; set; }
    public bool IsOutOfStock { get; set; }
}
