namespace MarketPOS.Shared.DTOs.BaseDtoAndBaseAuditableDtoAndConContactInfoDto;

/// <summary>
/// Represents a base data transfer object (DTO) that provides common properties for tracking  entity metadata, such as
/// creation, modification, deletion, and restoration details.
/// </summary>
/// <remarks>This abstract class is intended to be inherited by other DTO classes to standardize  metadata
/// properties across the application. It includes properties for identifying the  entity, tracking its creation and
/// modification timestamps, and managing soft deletion  and restoration metadata.</remarks>
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
    [JsonConverter(typeof(JsonNullableDateConverter))]
    public DateTime? RestoreAt { get; set; }
    [JsonPropertyOrder(27)]
    public string? RestoreBy { get; set; }
}






