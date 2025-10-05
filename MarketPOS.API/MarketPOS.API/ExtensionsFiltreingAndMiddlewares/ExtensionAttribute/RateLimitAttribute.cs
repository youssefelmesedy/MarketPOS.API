namespace MarketPOS.API.ExtensionsFiltreingAndMiddlewares.ExtensionAttribute;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
public class RateLimitAttribute : Attribute
{
    // Dictionary<Role, (Limit, Seconds)>
    public Dictionary<string, (int Limit, int Seconds)> RoleLimits { get; }

    /// <summary>
    /// مثال الاستخدام:
    /// [RateLimit("User:5:10", "Guest:2:10", "Admin:50:60")]
    /// </summary>
    /// <param name="roleLimits">صيغة: "Role:Limit:Seconds"</param>
    public RateLimitAttribute(params string[] roleLimits)
    {
        RoleLimits = new Dictionary<string, (int Limit, int Seconds)>(StringComparer.OrdinalIgnoreCase);

        foreach (var rl in roleLimits)
        {
            // الشكل: "Role:Limit:Seconds"
            var parts = rl.Split(':');
            if (parts.Length == 3 &&
                int.TryParse(parts[1], out var limit) &&
                int.TryParse(parts[2], out var seconds))
            {
                RoleLimits[parts[0]] = (limit, seconds);
            }
        }
    }
}

