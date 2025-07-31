namespace MarketPOS.Shared.DTOs.BaseDtoAndBaseAuditableDtoAndConContactInfoDto;
public abstract class BaseDto
{
    [JsonPropertyOrder(0)]
    public Guid Id { get; set; }
    [JsonPropertyOrder(19)]
    public string? CreatedBy { get; set; }
    [JsonPropertyOrder(20)]
    [JsonConverter(typeof(JsonDateConverter))]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyOrder(21)]
    [JsonConverter(typeof(JsonNullableDateConverter))]
    public DateTime? UpdatedAt { get; set; }
    [JsonPropertyOrder(22)]
    public string? ModifiedBy { get; set; }
    [JsonPropertyOrder(23)]
    public bool IsDeleted { get; set; }
    [JsonPropertyOrder(24)]
    public string? DeleteBy { get; set; }
    [JsonPropertyOrder(25)]
    [JsonConverter(typeof(JsonNullableDateConverter))]
    public DateTime? DeletedAt { get; set; }
    [JsonPropertyOrder(26)]
<<<<<<< HEAD:MarketPOS.API/MarketPOS.Shared/DTOs/BaseDtoAndBaseAuditableDtoAndConContactInfoDto/BaseDto.cs
    public DateTime? RestoreAt { get; set; }
    [JsonPropertyOrder(27)]
=======
>>>>>>> origin:MarketPOS.Shared/DTOs/BaseDtoAndBaseAuditableDtoAndConContactInfoDto/BaseDto.cs
    public string? RestoreBy { get; set; }
}






