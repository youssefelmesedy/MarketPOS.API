namespace MarketPOS.Shared.DTOs.CategoryDto; 
public class WareHouseCreateDto 
{
    public string Name { get; set; } = string.Empty;

    public ContactInfoDto? ContactInfoDto { get; set; } = new ContactInfoDto();
}
public class WareHouseUpdateDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public ContactInfoDto? ContactInfoDto { get; set; } = new ContactInfoDto();
}
public class WareHouseDetailsDto : BaseDto
{
    [JsonPropertyOrder(2)]
    public string Name { get; set; } = string.Empty;

    public ContactInfoDto ContactInfoDto { get; set; } = new ContactInfoDto();
}
