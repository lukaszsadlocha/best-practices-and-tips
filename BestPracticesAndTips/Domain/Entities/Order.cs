using BestPracticesAndTips.Domain.Common;
using BestPracticesAndTips.Domain.Enums;

namespace BestPracticesAndTips.Domain.Entities;

public class Order : BaseEntity
{
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public string BillingAddress { get; set; } = string.Empty;
    public string? Notes { get; set; }
    public DateTime? ShippedDate { get; set; }
    
    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    
    // Domain methods
    public void CompleteOrder()
    {
        Status = OrderStatus.Completed;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void ShipOrder()
    {
        Status = OrderStatus.Shipped;
        ShippedDate = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public void CancelOrder()
    {
        Status = OrderStatus.Cancelled;
        UpdatedAt = DateTime.UtcNow;
    }
    
    public decimal CalculateTotalAmount()
    {
        return OrderItems.Sum(item => item.Quantity * item.UnitPrice);
    }
}
