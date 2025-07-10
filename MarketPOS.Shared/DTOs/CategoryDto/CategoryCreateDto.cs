using MarketPOS.Shared.DTOs.BaseDtoAndBaseAuditableDtoAndConContactInfoDto;

namespace MarketPOS.Shared.DTOs.CategoryDto;

public class CategoryCreateDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}
public class CategoryUpdateDto 
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
}
