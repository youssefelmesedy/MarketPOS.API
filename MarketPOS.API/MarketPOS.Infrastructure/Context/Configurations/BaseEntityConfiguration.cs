namespace MarketPOS.Infrastructure.Context.Configurations;
public abstract class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T>
    where T : BaseEntity
{
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.CreatedAt).IsRequired();
        builder.Property(e => e.UpdatedAt);
        builder.Property(e => e.IsDeleted).HasDefaultValue(false);
        builder.Property(e => e.DeletedAt);
        builder.Property(e => e.RestorAt);

        builder.Property(e => e.CreatedBy)
               .HasMaxLength(100);

        builder.Property(e => e.ModifiedBy)
               .HasMaxLength(100);

        builder.Property(e => e.DeleteBy)
              .HasMaxLength(100);

        builder.Property(e => e.RestorBy)
             .HasMaxLength(100);
    }
}


