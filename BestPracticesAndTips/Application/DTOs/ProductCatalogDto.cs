namespace BestPracticesAndTips.Application.DTOs;

public record ProductCatalogDto
{
    public IReadOnlyCollection<ProductDto> Products { get; set; } = [];
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages { get; set; }
    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
}

public record ProductCatalogFilterDto
{
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string? Category { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public bool InStockOnly { get; set; } = true;
    public string? SearchTerm { get; set; }
}
