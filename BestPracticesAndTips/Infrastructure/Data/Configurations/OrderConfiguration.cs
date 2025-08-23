using BestPracticesAndTips.Domain.Entities;
using BestPracticesAndTips.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BestPracticesAndTips.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(o => o.Id);
        
        builder.Property(o => o.OrderDate)
            .IsRequired();
            
        builder.Property(o => o.Status)
            .IsRequired()
            .HasConversion<int>();
            
        builder.Property(o => o.TotalAmount)
            .HasPrecision(18, 2);
            
        builder.Property(o => o.ShippingAddress)
            .IsRequired()
            .HasMaxLength(500);
            
        builder.Property(o => o.BillingAddress)
            .IsRequired()
            .HasMaxLength(500);
            
        builder.Property(o => o.Notes)
            .HasMaxLength(1000);
        
        // Relationships
        builder.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);
            
        builder.HasMany(o => o.OrderItems)
            .WithOne(oi => oi.Order)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
