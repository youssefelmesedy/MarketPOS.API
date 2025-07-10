namespace MarketPOS.Infrastructure.Exceptions;
public class ExtendedProblemDetails : ProblemDetails
{
    public bool Success { get; set; } = false;
    public string? ErrorCode { get; set; }
    public string? ErrorSource { get; set; }

    [JsonInclude] // ✅ لازم دي علشان System.Text.Json يسلسلها
    [JsonPropertyName("errors")] // ✅ اسم مخصص يظهر في JSON
    public Dictionary<string, string[]> Errors { get; set; } = new(); // ✅ متتخليش null
}

