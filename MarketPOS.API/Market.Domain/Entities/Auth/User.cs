using Microsoft.AspNetCore.Identity;
namespace Market.Domain.Entities.Auth;
public class User : IdentityUser<Guid>
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }

    // ✅ خاصية محسوبة (مش هتتخزن في DB) بتجمع الاسم الأول والأخير
    public string? FullName => $"{FirstName} {LastName}".Trim();

    // ✅ الحقول الخاصة بالتتبع (Audit Fields)
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string? CreatedBy { get; set; }
    public string? ModifiedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string? DeleteBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public DateTime? RestorAt { get; set; }
    public string? RestorBy { get; set; }

    // ✅ صورة البروفايل (مسار أو URL مش صورة Binary)
    public string? ProfileImageUrl { get; set; }

    // ✅ علاقة مع RefreshTokens
    public List<RefreshToken>? RefreshTokens { get; set; } = new();
}

