using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Application.UseCases.Customers;
using MediatR;

namespace BestPracticesAndTips.Presentation.Endpoints;

public static class CustomerEndpoints
{
    public static void MapCustomerEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/customers")
            .WithTags("Customers")
            .WithOpenApi();

        group.MapGet("/", async (IMediator mediator, bool activeOnly = true) =>
        {
            var query = new GetCustomersQuery { ActiveOnly = activeOnly };
            var customers = await mediator.Send(query);
            return Results.Ok(customers);
        })
        .WithName("GetCustomers")
        .WithSummary("Get all customers")
        .Produces<IEnumerable<CustomerDto>>();

        group.MapGet("/{id:int}", async (IMediator mediator, int id) =>
        {
            var query = new GetCustomerByIdQuery { Id = id };
            var customer = await mediator.Send(query);
            return customer is not null ? Results.Ok(customer) : Results.NotFound();
        })
        .WithName("GetCustomerById")
        .WithSummary("Get customer by ID")
        .Produces<CustomerDto>()
        .Produces(404);

        group.MapPost("/", async (IMediator mediator, CreateCustomerDto customerDto) =>
        {
            try
            {
                var command = new CreateCustomerCommand { Customer = customerDto };
                var customer = await mediator.Send(command);
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
    }
}
