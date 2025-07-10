using MarketPOS.Shared.DTOs.BaseDtoAndBaseAuditableDtoAndConContactInfoDto;

namespace MarketPOS.Shared.DTOs.SupplierDto;

public class SupplierDetailsDto : BaseDto
{
    public string? Name { get; set; }

    public ContactInfoDto? contactInfoDtos { get; set; }
}
