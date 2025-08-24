using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Enums;

namespace BestPracticesAndTips.Application.UseCases.Reports;

public class GetSalesReportUseCase(IOrderRepository orderRepository) : IGetSalesReportUseCase
{
    public async Task<SalesReportDto> ExecuteAsync(SalesReportFilterDto filter, CancellationToken cancellationToken = default)
    {
        // Get completed orders within the date range
        var orders = await orderRepository.GetOrdersInDateRangeAsync(filter.StartDate, filter.EndDate);
        var completedOrders = orders.Where(o => o.Status == OrderStatus.Completed).ToList();

        var totalRevenue = completedOrders.Sum(o => o.TotalAmount);
        var totalOrders = completedOrders.Count;
        var averageOrderValue = totalOrders > 0 ? totalRevenue / totalOrders : 0;

        // Generate sales periods based on grouping
        var salesPeriods = GenerateSalesPeriods(completedOrders, filter.GroupBy, filter.StartDate, filter.EndDate);

        // Get top products
        var topProducts = completedOrders
            .SelectMany(o => o.OrderItems)
            .GroupBy(oi => new { oi.ProductId, oi.Product?.Name, oi.Product?.Category })
            .Select(g => new TopProductDto
            {
                ProductId = g.Key.ProductId,
                ProductName = g.Key.Name ?? "Unknown",
                Category = g.Key.Category ?? "Unknown",
                QuantitySold = g.Sum(oi => oi.Quantity),
                Revenue = g.Sum(oi => oi.Quantity * oi.UnitPrice)
            })
            .OrderByDescending(tp => tp.Revenue)
            .Take(10)
            .ToList();

        // Generate customer segments
        var customerSegments = GenerateCustomerSegments(completedOrders);

        return new SalesReportDto
        {
            StartDate = filter.StartDate,
            EndDate = filter.EndDate,
            GroupBy = filter.GroupBy,
            TotalRevenue = totalRevenue,
            TotalOrders = totalOrders,
            AverageOrderValue = averageOrderValue,
            SalesPeriods = salesPeriods,
            TopProducts = topProducts,
            CustomerSegments = customerSegments
        };
    }

    private static List<SalesPeriodDto> GenerateSalesPeriods(List<Domain.Entities.Order> orders, string groupBy, DateTime startDate, DateTime endDate)
    {
        var periods = new List<SalesPeriodDto>();

        switch (groupBy.ToLower())
        {
            case "daily":
                for (var date = startDate.Date; date <= endDate.Date; date = date.AddDays(1))
                {
                    var periodOrders = orders.Where(o => o.OrderDate.Date == date).ToList();
                    periods.Add(CreateSalesPeriod(date, date.AddDays(1).AddTicks(-1), date.ToString("yyyy-MM-dd"), periodOrders));
                }
                break;

            case "weekly":
                var weekStart = startDate.Date;
                while (weekStart <= endDate.Date)
                {
                    var weekEnd = weekStart.AddDays(6);
                    if (weekEnd > endDate.Date) weekEnd = endDate.Date;
                    
                    var periodOrders = orders.Where(o => o.OrderDate.Date >= weekStart && o.OrderDate.Date <= weekEnd).ToList();
                    periods.Add(CreateSalesPeriod(weekStart, weekEnd.AddDays(1).AddTicks(-1), $"Week of {weekStart:yyyy-MM-dd}", periodOrders));
                    
                    weekStart = weekStart.AddDays(7);
                }
                break;

            case "monthly":
                var monthStart = new DateTime(startDate.Year, startDate.Month, 1);
                while (monthStart <= endDate.Date)
                {
                    var monthEnd = monthStart.AddMonths(1).AddDays(-1);
                    if (monthEnd > endDate.Date) monthEnd = endDate.Date;
                    
                    var periodOrders = orders.Where(o => o.OrderDate.Date >= monthStart && o.OrderDate.Date <= monthEnd).ToList();
                    periods.Add(CreateSalesPeriod(monthStart, monthEnd.AddDays(1).AddTicks(-1), monthStart.ToString("yyyy-MM"), periodOrders));
                    
                    monthStart = monthStart.AddMonths(1);
                }
                break;
        }

        return periods;
    }

    private static SalesPeriodDto CreateSalesPeriod(DateTime start, DateTime end, string label, List<Domain.Entities.Order> orders)
    {
        var revenue = orders.Sum(o => o.TotalAmount);
        var orderCount = orders.Count;
        
        return new SalesPeriodDto
        {
            PeriodStart = start,
            PeriodEnd = end,
            PeriodLabel = label,
            Revenue = revenue,
            OrderCount = orderCount,
            AverageOrderValue = orderCount > 0 ? revenue / orderCount : 0
        };
    }

    private List<CustomerSegmentDto> GenerateCustomerSegments(List<Domain.Entities.Order> orders)
    {
        var customerTotals = orders
            .GroupBy(o => o.CustomerId)
            .Select(g => new { CustomerId = g.Key, TotalSpent = g.Sum(o => o.TotalAmount) })
            .ToList();

        if (!customerTotals.Any())
        {
            return [];
        }

        var maxSpent = customerTotals.Max(c => c.TotalSpent);
        var segments = new List<CustomerSegmentDto>();

        // High value customers (top 20% by spending)
        var highValueThreshold = maxSpent * 0.8m;
        var highValueCustomers = customerTotals.Where(c => c.TotalSpent >= highValueThreshold).ToList();
        if (highValueCustomers.Any())
        {
            segments.Add(new CustomerSegmentDto
            {
                SegmentName = "High Value",
                CustomerCount = highValueCustomers.Count,
                TotalRevenue = highValueCustomers.Sum(c => c.TotalSpent),
                AverageCustomerValue = highValueCustomers.Average(c => c.TotalSpent)
            });
        }

        // Medium value customers (20-80% range)
        var mediumValueThreshold = maxSpent * 0.2m;
        var mediumValueCustomers = customerTotals.Where(c => c.TotalSpent >= mediumValueThreshold && c.TotalSpent < highValueThreshold).ToList();
        if (mediumValueCustomers.Any())
        {
            segments.Add(new CustomerSegmentDto
            {
                SegmentName = "Medium Value",
                CustomerCount = mediumValueCustomers.Count,
                TotalRevenue = mediumValueCustomers.Sum(c => c.TotalSpent),
                AverageCustomerValue = mediumValueCustomers.Average(c => c.TotalSpent)
            });
        }

        // Low value customers (bottom 20%)
        var lowValueCustomers = customerTotals.Where(c => c.TotalSpent < mediumValueThreshold).ToList();
        if (lowValueCustomers.Any())
        {
            segments.Add(new CustomerSegmentDto
            {
                SegmentName = "Low Value",
                CustomerCount = lowValueCustomers.Count,
                TotalRevenue = lowValueCustomers.Sum(c => c.TotalSpent),
                AverageCustomerValue = lowValueCustomers.Average(c => c.TotalSpent)
            });
        }

        return segments;
    }
}
