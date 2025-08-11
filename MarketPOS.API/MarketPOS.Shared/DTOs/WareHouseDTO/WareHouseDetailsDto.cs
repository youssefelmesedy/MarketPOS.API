namespace MarketPOS.Shared.DTOs.WareHouseDTO;

public class WareHouseDetailsDto : BaseDto
{
    [JsonPropertyOrder(2)]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyOrder(3)]
    public ContactInfoDto ContactInfoDto { get; set; } = new ContactInfoDto();
}
