using System.ComponentModel.DataAnnotations;

namespace MarketPOS.Shared.DTOs.AuthenticationDTO;
public record ChangePasswordDto
{
    [Required]
    public Guid UserId { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string? CurrentPassword { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string? NewPassword { get; set; }
    [Required]
    [DataType(DataType.Password)]
    public string? ConfirmNewPassword { get; set; }
}
