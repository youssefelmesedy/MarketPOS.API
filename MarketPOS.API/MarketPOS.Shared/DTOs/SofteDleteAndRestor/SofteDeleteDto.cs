namespace MarketPOS.Shared.DTOs.SofteDleteAndRestor;

public class SofteDeleteDto
{
    public Guid Id { get; set; }
    public string? name { get; init; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}
