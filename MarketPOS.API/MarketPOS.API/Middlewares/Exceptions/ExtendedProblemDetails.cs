using System.Text.Json.Serialization;

namespace MarketPOS.Infrastructure.Exceptions;
public class ExtendedProblemDetails : ProblemDetails
{
    public bool Success { get; set; } = false;
    public string? ErrorCode { get; set; }
    public string? ErrorSource { get; set; }

    [JsonInclude]
    [JsonPropertyName("errors")]
    public object? Errors { get; set; }
}



