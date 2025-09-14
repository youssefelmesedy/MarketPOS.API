namespace MarketPOS.Shared.DTOs.ProductDto;
public class SomeFeaturesProductDto : BaseDto
{
    [JsonPropertyOrder(1)]
    public string? Name { get; set; }
    [JsonPropertyOrder(2)]
    public string? Barcode { get; set; }
    [JsonPropertyOrder(3)]
    public List<Guid>? IngredinentId { get; set; }
    [JsonPropertyOrder(4)]
    public List<string>? IngredinentName { get; set; }
    [JsonPropertyOrder(5)]
    public Guid CategoryId { get; set; }
    [JsonPropertyOrder(6)]
    public string? CategoryName { get; set; }

    [JsonPropertyOrder(7)]
    public string? WaerHousId { get; set; }
    [JsonPropertyOrder(8)]
    public string? WaerHousName { get; set; }
    [JsonPropertyOrder(9)]
    public DateTime? ExpirationDate { get; set; }

    // Prices
    [JsonPropertyOrder(10)]
    public decimal PurchasePrice { get; set; }
    [JsonPropertyOrder(11)]
    public decimal SalePrice { get; set; }
    [JsonPropertyOrder(12)]
    public decimal DiscountPercentageFromSupplier { get; set; }
}
