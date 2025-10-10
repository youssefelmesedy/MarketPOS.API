using System.ComponentModel.DataAnnotations;

namespace MarketPOS.Shared.DTOs.AuthenticationDTO;
public record RequestPasswordResetDto
{
    [Required]
    [EmailAddress]
    public string? Email { get; init; }
    [Required]
    public bool ServerHosted { get; set; } = false;
}

