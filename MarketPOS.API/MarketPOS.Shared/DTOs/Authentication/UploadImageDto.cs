using MarketPOS.Application.Common.ExtensionsFile;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace  MarketPOS.Shared.DTOs.Authentication;

public class UploadImageDto
{
    [Required, DataType(DataType.CreditCard)]
    public Guid UserId { get; set; }

    [Required]
    public string? PersoneFullName { get; set; }

    [Required]
    [MaxFileSize(5 * 1024 * 1024)] // 5 MB
    [AllowedExtensions(new[] { ".jpg", ".jpeg", ".png" })]
    public IFormFile? File { get; set; }
}
