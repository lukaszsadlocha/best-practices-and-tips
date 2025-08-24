using BestPracticesAndTips.Application.Common.Interfaces;
using BestPracticesAndTips.Application.Common.Interfaces.UseCases;
using BestPracticesAndTips.Domain.Enums;

namespace BestPracticesAndTips.Application.UseCases.Products;

public class DeleteProductUseCase(IProductRepository productRepository, IOrderRepository orderRepository) : IDeleteProductUseCase
{
    public async Task<bool> ExecuteAsync(int id, CancellationToken cancellationToken = default)
    {
        var product = await productRepository.GetByIdAsync(id);
        if (product == null)
        {
            throw new InvalidOperationException($"Product with ID {id} not found.");
        }

        if (!product.IsActive)
        {
            throw new InvalidOperationException("Product is already inactive.");
        }

        // Check for pending orders - cannot delete products with pending orders
        var pendingOrders = await orderRepository.GetOrdersByStatusAsync(OrderStatus.Pending);
        var hasPendingOrders = pendingOrders.Any(o => o.OrderItems.Any(oi => oi.ProductId == id));
        
        if (hasPendingOrders)
        {
            throw new InvalidOperationException("Cannot delete product with pending orders. Please complete or cancel all pending orders first.");
        }

        // Soft delete using domain method
        product.Deactivate();
        await productRepository.UpdateAsync(product);

        return true;
    }
}
