namespace MarketPOS.Shared.DTOs.SofteDleteAndRestor;

public class RestorDto
{
    public Guid Id { get; set; }
    public string? name { get; init; }
    public String? RestoredBy { get; set; }
    public DateTime? RestoredAt { get; set; }
}
