using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.UseCases.Customers;
using BestPracticesAndTips.Application.UseCases.Orders;
using BestPracticesAndTips.Application.UseCases.Products;
using BestPracticesAndTips.Application.UseCases.Reports;
using BestPracticesAndTips.Domain.Services;
using BestPracticesAndTips.Infrastructure.Data;
using BestPracticesAndTips.Infrastructure.Repositories;
using BestPracticesAndTips.Presentation.Endpoints;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add Entity Framework with In-Memory Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("BestPracticesDb"));

// Add Repositories
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();

// Add Domain Services
builder.Services.AddScoped<IOrderDomainService, OrderDomainService>();

// Register Customer Use Case Services
builder.Services.AddScoped<ICreateCustomerUseCase, CreateCustomerUseCase>();
builder.Services.AddScoped<IGetCustomerByIdUseCase, GetCustomerByIdUseCase>();
builder.Services.AddScoped<IGetCustomersUseCase, GetCustomersUseCase>();
builder.Services.AddScoped<IUpdateCustomerUseCase, UpdateCustomerUseCase>();
builder.Services.AddScoped<IDeleteCustomerUseCase, DeleteCustomerUseCase>();
builder.Services.AddScoped<IGetCustomerAnalyticsUseCase, GetCustomerAnalyticsUseCase>();

// Register Product Use Case Services
builder.Services.AddScoped<IGetProductsUseCase, GetProductsUseCase>();
builder.Services.AddScoped<IGetProductByIdUseCase, GetProductByIdUseCase>();
builder.Services.AddScoped<ICreateProductUseCase, CreateProductUseCase>();
builder.Services.AddScoped<IUpdateProductUseCase, UpdateProductUseCase>();
builder.Services.AddScoped<IDeleteProductUseCase, DeleteProductUseCase>();
builder.Services.AddScoped<IUpdateStockUseCase, UpdateStockUseCase>();
builder.Services.AddScoped<IGetProductCatalogUseCase, GetProductCatalogUseCase>();
builder.Services.AddScoped<IGetLowStockProductsUseCase, GetLowStockProductsUseCase>();

// Register Order Use Case Services
builder.Services.AddScoped<IGetOrdersUseCase, GetOrdersUseCase>();
builder.Services.AddScoped<ICreateOrderUseCase, CreateOrderUseCase>();
builder.Services.AddScoped<IUpdateOrderUseCase, UpdateOrderUseCase>();
builder.Services.AddScoped<ICancelOrderUseCase, CancelOrderUseCase>();
builder.Services.AddScoped<IShipOrderUseCase, ShipOrderUseCase>();
builder.Services.AddScoped<ICompleteOrderUseCase, CompleteOrderUseCase>();
builder.Services.AddScoped<IGetOrderStatusHistoryUseCase, GetOrderStatusHistoryUseCase>();

// Register Report Use Case Services
builder.Services.AddScoped<IGetSalesReportUseCase, GetSalesReportUseCase>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await DatabaseSeeder.SeedAsync(context);
}

// Map API endpoints
app.MapCustomerEndpoints();
app.MapProductEndpoints();
app.MapOrderEndpoints();
app.MapReportEndpoints();
app.MapTestEndpoints();

app.Run();
