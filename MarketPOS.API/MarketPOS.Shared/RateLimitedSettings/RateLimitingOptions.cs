namespace MarketPOS.Shared.RateLimitedSettings;

public class RateLimitingOptions
{
    public RateLimitRule Default { get; set; } = new();
    public Dictionary<string, RateLimitRule> Admins { get; set; } = new();
    public Dictionary<string, RateLimitRule> Users { get; set; } = new();
    public Dictionary<string, RateLimitRule> Guests { get; set; } = new();
    public Dictionary<string, RateLimitRule> SpecialRoles { get; set; } = new();
}

public class RateLimitRule
{
    public int MaxRequests { get; set; }
    public int TimeWindowSeconds { get; set; }
}

