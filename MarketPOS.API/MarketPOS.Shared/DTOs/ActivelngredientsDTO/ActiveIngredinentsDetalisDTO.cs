namespace MarketPOS.Shared.DTOs.ActivelngredientsDTO;

public class ActiveIngredinentsDetalisDTO : BaseDto
{
    [JsonPropertyOrder(2)]
    public string Name { get; set; } = string.Empty;
}
