using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Application.UseCases.Orders;
using BestPracticesAndTips.Domain.Enums;
using MediatR;

namespace BestPracticesAndTips.Presentation.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/orders")
            .WithTags("Orders")
            .WithOpenApi();

        group.MapGet("/", async (IMediator mediator, int? customerId = null, OrderStatus? status = null, bool recent = false) =>
        {
            var query = new GetOrdersQuery 
            { 
                CustomerId = customerId,
                Status = status,
                RecentOnly = recent
            };
            var orders = await mediator.Send(query);
            return Results.Ok(orders);
        })
        .WithName("GetOrders")
        .WithSummary("Get all orders")
        .Produces<IEnumerable<OrderDto>>();

        group.MapGet("/customer/{customerId:int}", async (IMediator mediator, int customerId) =>
        {
            var query = new GetOrdersQuery { CustomerId = customerId };
            var orders = await mediator.Send(query);
            return Results.Ok(orders);
        })
        .WithName("GetOrdersByCustomer")
        .WithSummary("Get orders by customer ID")
        .Produces<IEnumerable<OrderDto>>();

        group.MapGet("/status/{status}", async (IMediator mediator, OrderStatus status) =>
        {
            var query = new GetOrdersQuery { Status = status };
            var orders = await mediator.Send(query);
            return Results.Ok(orders);
        })
        .WithName("GetOrdersByStatus")
        .WithSummary("Get orders by status")
        .Produces<IEnumerable<OrderDto>>();
    }
}
