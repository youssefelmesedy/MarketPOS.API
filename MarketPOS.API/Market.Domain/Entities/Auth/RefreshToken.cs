namespace Market.Domain.Entities.Auth;
public class RefreshToken
{
    public Guid Id { get; set; }
    public string? RefToken { get; set; }
    public string? Device { get; set; } // User Agent
    public string? IpAddress { get; set; } 
    public DateTime ExpiresAt { get; set; }
    public DateTime? Revoked { get; set; } 
    public DateTime CreatedAt { get; set; }

    public bool IsExpired  => DateTime.UtcNow >= ExpiresAt;
    public bool IsActive => Revoked == null && !IsExpired;

    // Navigation Properties
    public Guid UserId { get; set; }
    public User? User { get; set; }

}
