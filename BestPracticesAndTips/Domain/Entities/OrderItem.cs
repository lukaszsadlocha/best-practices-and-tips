using BestPracticesAndTips.Domain.Common;

namespace BestPracticesAndTips.Domain.Entities;

public class OrderItem : BaseEntity
{
    public int OrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    
    // Navigation properties
    public virtual Order Order { get; set; } = null!;
    public virtual Product Product { get; set; } = null!;
    
    // Domain methods
    public decimal GetSubTotal() => Quantity * UnitPrice;
}
