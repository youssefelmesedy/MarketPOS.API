namespace MarketPOS.Infrastructure.Context.Configurations;
public class CategoryConfiguration : BaseEntityConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        base.Configure(builder);

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(c => c.Description)
               .HasMaxLength(100);

    }
}

public class ActiveIngredientsConfiguration : BaseEntityConfiguration<ActiveIngredients>
{
    public override void Configure(EntityTypeBuilder<ActiveIngredients> builder)
    {
        base.Configure(builder);

        builder.ToTable("ActiveIngredinents");

        builder.Property(c => c.Name)
               .IsRequired()
               .HasMaxLength(100);

        builder.HasMany(c => c.ProductIngredient)
               .WithOne(pa => pa.ActiveIngredinents)
               .HasForeignKey(pa => pa.ActiveIngredinentsId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}




