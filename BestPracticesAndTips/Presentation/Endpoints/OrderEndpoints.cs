using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Enums;

namespace BestPracticesAndTips.Presentation.Endpoints;

public static class OrderEndpoints
{
    public static void MapOrderEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/orders")
            .WithTags("Orders")
            .WithOpenApi();

        group.MapGet("/", async (IGetOrdersUseCase useCase, int? customerId = null, OrderStatus? status = null, bool recent = false) =>
        {
            var orders = await useCase.ExecuteAsync(customerId, status, recent);
            return Results.Ok(orders);
        })
        .WithName("GetOrders")
        .WithSummary("Get all orders")
        .Produces<IEnumerable<OrderDto>>();

        group.MapGet("/customer/{customerId:int}", async (IGetOrdersUseCase useCase, int customerId) =>
        {
            var orders = await useCase.ExecuteAsync(customerId);
            return Results.Ok(orders);
        })
        .WithName("GetOrdersByCustomer")
        .WithSummary("Get orders by customer ID")
        .Produces<IEnumerable<OrderDto>>();

        group.MapGet("/status/{status}", async (IGetOrdersUseCase useCase, OrderStatus status) =>
        {
            var orders = await useCase.ExecuteAsync(null, status);
            return Results.Ok(orders);
        })
        .WithName("GetOrdersByStatus")
        .WithSummary("Get orders by status")
        .Produces<IEnumerable<OrderDto>>();

        group.MapPost("/", async (ICreateOrderUseCase useCase, CreateOrderDto orderDto) =>
        {
            try
            {
                var order = await useCase.ExecuteAsync(orderDto);
                return Results.Created($"/api/orders/{order.Id}", order);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CreateOrder")
        .WithSummary("Create a new order")
        .Produces<OrderDto>(201)
        .Produces(400);

        group.MapPut("/{id:int}", async (IUpdateOrderUseCase useCase, int id, UpdateOrderDto orderDto) =>
        {
            try
            {
                var order = await useCase.ExecuteAsync(id, orderDto);
                return Results.Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("UpdateOrder")
        .WithSummary("Update order details (pending orders only)")
        .Produces<OrderDto>()
        .Produces(400)
        .Produces(404);

        group.MapPost("/{id:int}/cancel", async (ICancelOrderUseCase useCase, int id) =>
        {
            try
            {
                var order = await useCase.ExecuteAsync(id);
                return Results.Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CancelOrder")
        .WithSummary("Cancel an order and restore inventory")
        .Produces<OrderDto>()
        .Produces(400)
        .Produces(404);

        group.MapPost("/{id:int}/ship", async (IShipOrderUseCase useCase, int id) =>
        {
            try
            {
                var order = await useCase.ExecuteAsync(id);
                return Results.Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("ShipOrder")
        .WithSummary("Mark order as shipped")
        .Produces<OrderDto>()
        .Produces(400)
        .Produces(404);

        group.MapPost("/{id:int}/complete", async (ICompleteOrderUseCase useCase, int id) =>
        {
            try
            {
                var order = await useCase.ExecuteAsync(id);
                return Results.Ok(order);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CompleteOrder")
        .WithSummary("Mark order as completed")
        .Produces<OrderDto>()
        .Produces(400)
        .Produces(404);

        group.MapGet("/{id:int}/status-history", async (IGetOrderStatusHistoryUseCase useCase, int id) =>
        {
            var statusHistory = await useCase.ExecuteAsync(id);
            return statusHistory is not null ? Results.Ok(statusHistory) : Results.NotFound();
        })
        .WithName("GetOrderStatusHistory")
        .WithSummary("Get order status change history")
        .Produces<OrderStatusHistoryDto>()
        .Produces(404);
    }
}
