namespace MarketPOS.Infrastructure.Context.Configurations;
#region Product Configuration

public class ProductConfiguration : BaseEntityConfiguration<Product>
{
    public override void Configure(EntityTypeBuilder<Product> builder)
    {
        base.Configure(builder);

        builder.Property(p => p.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(p => p.Barcode)
               .HasMaxLength(50);

        builder.HasOne(p => p.Category)
               .WithMany(c => c.Products)
               .HasForeignKey(p => p.CategoryId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.ProductPrice)
               .WithOne(pp => pp.Product)
               .HasForeignKey<ProductPrice>(pp => pp.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.ProductUnitProfile)
               .WithOne(pu => pu.Product)
               .HasForeignKey<ProductUnitProfile>(pu => pu.ProductId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.ProductInventories)
               .WithOne(pi => pi.Product)
               .HasForeignKey(pi => pi.ProductId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}


#endregion

#region ProductPrice Configuration

public class ProductPriceConfiguration : BaseEntityConfiguration<ProductPrice>
{
    public override void Configure(EntityTypeBuilder<ProductPrice> builder)
    {
        builder.HasKey(p => p.ProductId);

        builder.Ignore(p => p.Id);
        builder.Ignore(p => p.DeletedAt);
        builder.Ignore(p => p.IsDeleted);
        builder.Ignore(p => p.RestorAt);
        builder.Ignore(p => p.RestorBy);
        builder.Ignore(p => p.DeleteBy);

        builder.Property(p => p.PurchasePrice).HasPrecision(18, 2);
        builder.Property(p => p.SalePrice).HasPrecision(18, 2);
        builder.Property(p => p.DiscountPercentageFromSupplier).HasPrecision(5, 2);
    }

}


#endregion

#region ProductUnitProfile Configuration

public class ProductUnitProfileConfiguration : BaseEntityConfiguration<ProductUnitProfile>
{
    public override void Configure(EntityTypeBuilder<ProductUnitProfile> builder)
    {
        builder.HasKey(p => p.ProductId);

        builder.Ignore(p => p.Id);
        builder.Ignore(p => p.DeletedAt);
        builder.Ignore(p => p.IsDeleted);
        builder.Ignore(p => p.RestorAt);
        builder.Ignore(p => p.RestorBy);
        builder.Ignore(p => p.DeleteBy);

        builder.Property(p => p.LargeUnitName).HasMaxLength(50).IsRequired();
        builder.Property(p => p.MediumUnitName).HasMaxLength(50).IsRequired();
        builder.Property(p => p.SmallUnitName).HasMaxLength(50).IsRequired();

        builder.Property(p => p.LargeUnitPrice).HasPrecision(18, 2);
        builder.Property(p => p.MediumUnitPrice).HasPrecision(18, 2);
        builder.Property(p => p.SmallUnitPrice).HasPrecision(18, 2);
    }
}


#endregion

#region ProductInventory Configuration

public class ProductInventoryConfiguration : BaseEntityConfiguration<ProductInventory>
{
    public override void Configure(EntityTypeBuilder<ProductInventory> builder)
    {
        base.Configure(builder);

        builder.Property(pi => pi.Quantity).IsRequired();

        builder.HasOne(pi => pi.Product)
               .WithMany(p => p.ProductInventories)
               .HasForeignKey(pi => pi.ProductId)
               .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(pi => pi.Warehouse)
               .WithMany(w => w.ProductInventories)
               .HasForeignKey(pi => pi.WarehouseId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}


#endregion

#region Warehouse Configuration

public class WarehouseConfiguration : BaseEntityConfiguration<Warehouse>
{
    public override void Configure(EntityTypeBuilder<Warehouse> builder)
    {
        base.Configure(builder);

        builder.Property(w => w.Name)
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


#endregion




