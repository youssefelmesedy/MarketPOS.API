using MarketPOS.Shared.DTOs.BaseDtoAndBaseAuditableDtoAndConContactInfoDto;
using System.Text.Json.Serialization;

namespace MarketPOS.Shared.DTOs.CategoryDto;

public class CategoryDetalisDto : BaseDto
{
    [JsonPropertyOrder(1)]
    public string? Name { get; set; }
    [JsonPropertyOrder(2)]
    public string? Description { get; set; }
}
