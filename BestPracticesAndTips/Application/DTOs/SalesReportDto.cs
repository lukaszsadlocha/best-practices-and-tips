namespace BestPracticesAndTips.Application.DTOs;

public record SalesReportDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string GroupBy { get; set; } = string.Empty;
    public decimal TotalRevenue { get; set; }
    public int TotalOrders { get; set; }
    public decimal AverageOrderValue { get; set; }
    public IReadOnlyCollection<SalesPeriodDto> SalesPeriods { get; set; } = [];
    public IReadOnlyCollection<TopProductDto> TopProducts { get; set; } = [];
    public IReadOnlyCollection<CustomerSegmentDto> CustomerSegments { get; set; } = [];
}

public record SalesPeriodDto
{
    public DateTime PeriodStart { get; set; }
    public DateTime PeriodEnd { get; set; }
    public string PeriodLabel { get; set; } = string.Empty;
    public decimal Revenue { get; set; }
    public int OrderCount { get; set; }
    public decimal AverageOrderValue { get; set; }
}

public record TopProductDto
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal Revenue { get; set; }
}

public record CustomerSegmentDto
{
    public string SegmentName { get; set; } = string.Empty;
    public int CustomerCount { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageCustomerValue { get; set; }
}

public record SalesReportFilterDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string GroupBy { get; set; } = "daily"; // daily, weekly, monthly
}
