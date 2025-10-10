using MarketPOS.Application.Common.ExtensionsFile;
using MarketPOS.Shared.Eunms.RolesEunm;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace  MarketPOS.Shared.DTOs.Authentication;
public record RegisterationDto
{
    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(55, ErrorMessage = "First name cannot exceed 55 characters.")]
    public string FirstName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(55, ErrorMessage = "Last name cannot exceed 55 characters.")]
    public string LastName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Username is required.")]
    [MaxLength(30, ErrorMessage = "Username cannot exceed 30 characters.")]
    public string UserName { get; init; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address format.")]
    public string Email { get; init; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    [MinLength(8, ErrorMessage = "Password must be at least 8 characters.")]
    //[RegularExpression(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
    //    ErrorMessage = "Password must contain uppercase, lowercase, number, and special character.")]
    public string Password { get; init; } = string.Empty;

    [Required(ErrorMessage = "Confirm Password is required.")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; init; } = string.Empty;

    [Required(ErrorMessage = "Gmail is required.")]
    [EmailAddress(ErrorMessage = "Invalid gmail address format.")]
    public string Gmail { get; init; } = string.Empty;

    [MaxFileSize(5 * 1024 * 1024)] // 5 MB
    [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png" })]
    public IFormFile? File { get; set; }

}
