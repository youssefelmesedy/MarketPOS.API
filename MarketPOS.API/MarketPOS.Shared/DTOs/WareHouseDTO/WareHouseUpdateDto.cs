namespace MarketPOS.Shared.DTOs.WareHouseDTO;
public class WareHouseUpdateDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ContactInfoDto? ContactInfoDto { get; set; } = new ContactInfoDto();
}
