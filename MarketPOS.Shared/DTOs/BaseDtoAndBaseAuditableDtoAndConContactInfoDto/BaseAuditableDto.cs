namespace MarketPOS.Shared.DTOs.BaseDtoAndBaseAuditableDtoAndConContactInfoDto;
public abstract class BaseAuditableDto : BaseDto
{
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}



