using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Application.DTOs;
using BestPracticesAndTips.Domain.Entities;
using BestPracticesAndTips.Domain.Enums;
using BestPracticesAndTips.Domain.Services;

namespace BestPracticesAndTips.Application.UseCases.Orders;

public class CreateOrderUseCase(
    IOrderRepository orderRepository, 
    ICustomerRepository customerRepository,
    IProductRepository productRepository,
    IOrderDomainService orderDomainService) : ICreateOrderUseCase
{
    public async Task<OrderDto> ExecuteAsync(CreateOrderDto orderDto, CancellationToken cancellationToken = default)
    {
        // Validate customer exists and is active
        var customer = await customerRepository.GetByIdAsync(orderDto.CustomerId);
        if (customer == null)
        {
            throw new InvalidOperationException($"Customer with ID {orderDto.CustomerId} not found.");
        }

        if (!customer.IsActive)
        {
            throw new InvalidOperationException("Cannot create order for inactive customer.");
        }

        if (!orderDto.OrderItems.Any())
        {
            throw new ArgumentException("Order must contain at least one item.");
        }

        var orderItems = new List<OrderItem>();
        decimal totalAmount = 0;

        // Validate and process order items
        foreach (var itemDto in orderDto.OrderItems)
        {
            var product = await productRepository.GetByIdAsync(itemDto.ProductId);
            if (product == null)
            {
                throw new InvalidOperationException($"Product with ID {itemDto.ProductId} not found.");
            }

            if (!product.IsActive)
            {
                throw new InvalidOperationException($"Product '{product.Name}' is not available.");
            }

            if (itemDto.Quantity <= 0)
            {
                throw new ArgumentException($"Quantity must be greater than zero for product '{product.Name}'.");
            }

            if (product.StockQuantity < itemDto.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for product '{product.Name}'. Available: {product.StockQuantity}, Requested: {itemDto.Quantity}");
            }

            var orderItem = new OrderItem
            {
                ProductId = product.Id,
                Quantity = itemDto.Quantity,
                UnitPrice = product.Price
            };

            orderItems.Add(orderItem);
            totalAmount += orderItem.Quantity * orderItem.UnitPrice;

            // Reserve stock (reduce from inventory)
            product.UpdateStock(-itemDto.Quantity);
            await productRepository.UpdateAsync(product);
        }

        // Create order using domain service for complex business rules
        var order = await orderDomainService.CreateOrderAsync(
            customer,
            orderItems,
            orderDto.ShippingAddress,
            orderDto.BillingAddress,
            orderDto.Notes
        );

        var createdOrder = await orderRepository.AddAsync(order);

        return MapToDto(createdOrder);
    }

    private static OrderDto MapToDto(Order order)
    {
        return new OrderDto
        {
            Id = order.Id,
            CustomerId = order.CustomerId,
            CustomerName = order.Customer?.GetFullName() ?? "Unknown",
            OrderDate = order.OrderDate,
            Status = order.Status.ToString(),
            TotalAmount = order.TotalAmount,
            ShippingAddress = order.ShippingAddress,
            BillingAddress = order.BillingAddress,
            Notes = order.Notes,
            ShippedDate = order.ShippedDate,
            OrderItems = order.OrderItems?.Select(oi => new OrderItemDto
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                ProductName = oi.Product?.Name ?? "Unknown",
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                SubTotal = oi.GetSubTotal()
            }).ToList() ?? []
        };
    }
}
