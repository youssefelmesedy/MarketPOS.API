namespace MarketPOS.Shared.DTOs.WareHouseDTO;

public class WareHouseCreateDto 
{
    public string Name { get; set; } = string.Empty;

    public ContactInfoDto? ContactInfoDto { get; set; } = new ContactInfoDto();
}
