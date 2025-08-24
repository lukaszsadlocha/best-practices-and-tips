using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;

namespace BestPracticesAndTips.Presentation.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/customers")
            .WithTags("Customers")
            .WithOpenApi();

        group.MapGet("/", async (IGetCustomersUseCase useCase, bool activeOnly = true) =>
        {
            var customers = await useCase.ExecuteAsync(activeOnly);
            return Results.Ok(customers);
        })
        .WithName("GetCustomers")
        .WithSummary("Get all customers")
        .Produces<IEnumerable<CustomerDto>>();

        group.MapGet("/{id:int}", async (IGetCustomerByIdUseCase useCase, int id) =>
        {
            var customer = await useCase.ExecuteAsync(id);
            return customer is not null ? Results.Ok(customer) : Results.NotFound();
        })
        .WithName("GetCustomerById")
        .WithSummary("Get customer by ID")
        .Produces<CustomerDto>()
        .Produces(404);

        group.MapPost("/", async (ICreateCustomerUseCase useCase, CreateCustomerDto customerDto) =>
        {
            try
            {
                var customer = await useCase.ExecuteAsync(customerDto);
                return Results.Created($"/api/customers/{customer.Id}", customer);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("CreateCustomer")
        .WithSummary("Create a new customer")
        .Produces<CustomerDto>(201)
        .Produces(400);

        group.MapPut("/{id:int}", async (IUpdateCustomerUseCase useCase, int id, UpdateCustomerDto customerDto) =>
        {
            try
            {
                var customer = await useCase.ExecuteAsync(id, customerDto);
                return Results.Ok(customer);
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("UpdateCustomer")
        .WithSummary("Update customer details")
        .Produces<CustomerDto>()
        .Produces(400)
        .Produces(404);

        group.MapDelete("/{id:int}", async (IDeleteCustomerUseCase useCase, int id) =>
        {
            try
            {
                var result = await useCase.ExecuteAsync(id);
                return result ? Results.NoContent() : Results.NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return Results.BadRequest(ex.Message);
            }
        })
        .WithName("DeleteCustomer")
        .WithSummary("Delete (deactivate) customer")
        .Produces(204)
        .Produces(400)
        .Produces(404);

        group.MapGet("/{id:int}/analytics", async (IGetCustomerAnalyticsUseCase useCase, int id) =>
        {
            var analytics = await useCase.ExecuteAsync(id);
            return analytics is not null ? Results.Ok(analytics) : Results.NotFound();
        })
        .WithName("GetCustomerAnalytics")
        .WithSummary("Get customer analytics and lifetime value")
        .Produces<CustomerAnalyticsDto>()
        .Produces(404);
    }
}
