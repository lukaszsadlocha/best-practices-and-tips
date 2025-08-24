using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Presentation.Endpoints;

public static class ReportEndpoints
{
    public static void MapReportEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/reports")
            .WithTags("Reports")
            .WithOpenApi();

        group.MapGet("/sales", async (IGetSalesReportUseCase useCase, 
            DateTime? startDate = null, DateTime? endDate = null, string groupBy = "daily") =>
        {
            var filter = new SalesReportFilterDto
            {
                StartDate = startDate ?? DateTime.UtcNow.AddDays(-30),
                EndDate = endDate ?? DateTime.UtcNow,
                GroupBy = groupBy
            };

            var report = await useCase.ExecuteAsync(filter);
            return Results.Ok(report);
        })
        .WithName("GetSalesReport")
        .WithSummary("Get sales report with analytics by period")
        .Produces<SalesReportDto>();
    }
}
