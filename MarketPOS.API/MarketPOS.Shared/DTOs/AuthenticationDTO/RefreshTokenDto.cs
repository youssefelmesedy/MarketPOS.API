namespace MarketPOS.Shared.DTOs.Authentication;
public record RefreshTokenDto
{
    public string Message { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;

    [JsonConverter(typeof(JsonNullableDateConverter))]
    public DateTime? ExpiredAt { get; set; }
}
