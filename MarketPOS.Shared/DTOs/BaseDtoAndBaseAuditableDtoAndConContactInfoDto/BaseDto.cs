using MarketPOS.Shared.DataConventer;
using System.Text.Json.Serialization;

namespace MarketPOS.Shared.DTOs.BaseDtoAndBaseAuditableDtoAndConContactInfoDto;

public abstract class BaseDto
{
    [JsonPropertyOrder(0)]
    public Guid Id { get; set; }
    [JsonPropertyOrder(19)]
    [JsonConverter(typeof(JsonDateConverter))]
    public DateTime CreatedAt { get; set; }
    [JsonPropertyOrder(20)]
    [JsonConverter(typeof(JsonNullableDateConverter))]
    public DateTime? UpdatedAt { get; set; }
    [JsonPropertyOrder(21)]
    public bool IsDeleted { get; set; }
    [JsonPropertyOrder(24)]
    [JsonConverter(typeof(JsonNullableDateConverter))]
    public DateTime? DeletedAt { get; set; }
    [JsonPropertyOrder(25)]
    public string? CreatedBy { get; set; }
    [JsonPropertyOrder(26)]
    public string? ModifiedBy { get; set; }
}






