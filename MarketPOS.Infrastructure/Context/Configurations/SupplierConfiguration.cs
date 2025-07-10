using Market.Domain.Entitys.PurchaseInvoices;
using Market.Domain.Entitys.Suppliers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace MarketPOS.Infrastructure.Context.Configurations;

public class SupplierConfiguration : BaseEntityConfiguration<Supplier>
{
    public override void Configure(EntityTypeBuilder<Supplier> builder)
    {
        base.Configure(builder);

        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.OwnsOne(s => s.ContactInfo, contact =>
        {
            contact.Property(c => c.Phone).HasMaxLength(20);
            contact.Property(c => c.Email).HasMaxLength(100);

            contact.OwnsOne(c => c.Address, address =>
            {
                address.Property(a => a.Country).HasMaxLength(50);
                address.Property(a => a.City).HasMaxLength(50);
                address.Property(a => a.Street).HasMaxLength(100);
            });
        });
    }
}

// This code configures the Supplier and PurchaseInvoice entities in the Entity Framework Core context.
public class PurchaseInvoiceConfiguration : BaseEntityConfiguration<PurchaseInvoice>
{
    public override void Configure(EntityTypeBuilder<PurchaseInvoice> builder)
    {
        base.Configure(builder);

        builder.HasOne(p => p.Supplier)
               .WithMany(s => s.PurchaseInvoices)
               .HasForeignKey(p => p.SupplierId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Warehouse)
               .WithMany()
               .HasForeignKey(p => p.WarehouseId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.TotalAmount).HasPrecision(18, 2);
        builder.Property(p => p.Notes).HasMaxLength(250);
    }
}
// This code configures the PurchaseInvoiceItem entity in the Entity Framework Core context.
public class PurchaseInvoiceItemConfiguration : BaseEntityConfiguration<PurchaseInvoiceItem>
{
    public override void Configure(EntityTypeBuilder<PurchaseInvoiceItem> builder)
    {
        base.Configure(builder);

        builder.HasOne(i => i.PurchaseInvoice)
               .WithMany(p => p.Items)
               .HasForeignKey(i => i.PurchaseInvoiceId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(i => i.Product)
               .WithMany()
               .HasForeignKey(i => i.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.Property(i => i.UnitName).IsRequired().HasMaxLength(50);
        builder.Property(i => i.UnitPrice).HasPrecision(18, 2);
    }
}



