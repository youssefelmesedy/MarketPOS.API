using Market.Domain.Entities.Auth;

namespace MarketPOS.Infrastructure.Context.Configurations.AuthConfiguration;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");

        // ✅ UserName لازم يكون موجود وفريد
        builder.HasIndex(u => u.UserName).IsUnique();
        builder.Property(u => u.UserName)
               .IsRequired()
               .HasMaxLength(256);

        // ✅ Email لازم يكون فريد
        builder.HasIndex(u => u.Email).IsUnique();
        builder.Property(u => u.Email)
               .IsRequired()
               .HasMaxLength(256);

        builder.HasIndex(u => u.Gmail).IsUnique();
        builder.Property(u => u.Gmail)
               .IsRequired()
               .HasMaxLength(256);

        builder.Property(u => u.FirstName)
               .HasMaxLength(100);

        builder.Property(u => u.LastName)
               .HasMaxLength(100);

        builder.Property(u => u.ProfileImageUrl)
               .HasMaxLength(2048);

        // ✅ نستخدم SQL Server GETUTCDATE() كـ default للـ CreatedAt
        builder.Property(u => u.CreatedAt)
               .HasDefaultValueSql("GETUTCDATE()");

        // ✅ UpdatedAt نسيبها nullable وتتحدث يدويًا أو عن طريق SaveChanges
        builder.Property(u => u.UpdatedAt)
               .IsRequired(false);

        // ✅ العلاقة مع RefreshTokens
        builder.HasMany(u => u.RefreshTokens)
               .WithOne(rt => rt.User)
               .HasForeignKey(rt => rt.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}

