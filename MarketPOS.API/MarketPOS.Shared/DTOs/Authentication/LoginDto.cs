namespace MarketPOS.Shared.DTOs.Authentication;

public record LoginDto
{
    public string EmailORUserName { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}