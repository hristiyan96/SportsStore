using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SportsStore.Core.Entities;

namespace SportsStore.Data.Configurations;

public class CustomerOrderConfiguration : IEntityTypeConfiguration<CustomerOrder>
{
    public void Configure(EntityTypeBuilder<CustomerOrder> builder)
    {
        builder.ToTable("Orders");

        builder.Property(o => o.CustomerName).HasMaxLength(120).IsRequired();
        builder.Property(o => o.Phone).HasMaxLength(40).IsRequired();
        builder.Property(o => o.ShippingAddress).HasMaxLength(240).IsRequired();
        builder.Property(o => o.City).HasMaxLength(80).IsRequired();
        builder.Property(o => o.Total).HasPrecision(18, 2);
        builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(20);

        builder.HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.Property(i => i.ProductName).HasMaxLength(140).IsRequired();
        builder.Property(i => i.UnitPrice).HasPrecision(18, 2);
        builder.Ignore(i => i.LineTotal);

        builder.ToTable(t => t.HasCheckConstraint("CK_OrderItem_Quantity", "[Quantity] > 0"));

        builder.HasOne(i => i.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ProductReviewConfiguration : IEntityTypeConfiguration<ProductReview>
{
    public void Configure(EntityTypeBuilder<ProductReview> builder)
    {
        builder.Property(r => r.Comment).HasMaxLength(1000).IsRequired();

        builder.ToTable(t => t.HasCheckConstraint("CK_ProductReview_Rating", "[Rating] >= 1 AND [Rating] <= 5"));

        builder.HasIndex(r => new { r.ProductId, r.UserId }).IsUnique();

        builder.HasOne(r => r.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(r => r.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(r => r.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
{
    public void Configure(EntityTypeBuilder<ApplicationUser> builder)
    {
        builder.Property(u => u.FirstName).HasMaxLength(60).IsRequired();
        builder.Property(u => u.LastName).HasMaxLength(60).IsRequired();
        builder.Ignore(u => u.FullName);
    }
}
