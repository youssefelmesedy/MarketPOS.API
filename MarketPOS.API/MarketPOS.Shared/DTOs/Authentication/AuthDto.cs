namespace MarketPOS.Shared.DTOs.Authentication;
public record AuthDto
{
    public bool IsAuthenticated { get; set; }
    public string Message { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public List<string>? Roles { get; set; }
    public string Token { get; set; } = string.Empty;
    [JsonConverter(typeof(JsonNullableDateConverter))]
    public DateTime? ExpiresAt { get; set; } = default;
    [JsonIgnore]
    public string RefreshToken { get; set; } = string.Empty;

    public string ProfileImageURL { get; set; } = string.Empty;
}
