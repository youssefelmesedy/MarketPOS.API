using Market.Domain.Entities.Auth;

namespace MarketPOS.Infrastructure.Context.Configurations.AuthConfiguration;
public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        //  Primery key
        builder.HasKey(rt => rt.Id);

        //  RefToken (Required + MaxLength)
        builder.Property(rt => rt.RefToken)
               .IsRequired()
               .HasMaxLength(500);

        builder.Property(rt => rt.Device)
               .HasMaxLength(255);

        builder.Property(rt => rt.IpAddress)
               .HasMaxLength(250);

        //  Expiry Date (Required)
        builder.Property(rt => rt.ExpiresAt)
               .IsRequired();

        //  CreatedAt
        builder.Property(r => r.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        //  Revoked
        builder.Property(r => r.Revoked)
            .HasDefaultValue(null);

        //  Relationship: User 1 → * RefreshTokens
        builder.HasOne(r => r.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        //  Index (RefToken + UserId) لسرعة البحث والتأكد من عدم التكرار
        builder.HasIndex(r => new { r.RefToken, r.UserId }).IsUnique();
    }
}
