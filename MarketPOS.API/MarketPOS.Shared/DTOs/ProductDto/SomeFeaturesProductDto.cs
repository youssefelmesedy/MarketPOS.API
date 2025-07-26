namespace MarketPOS.Shared.DTOs.ProductDto;
public class SomeFeaturesProductDto : BaseDto
{
    [JsonPropertyOrder(1)]
    public string? Name { get; set; }
    [JsonPropertyOrder(2)]
    public string? Barcode { get; set; }
    [JsonPropertyOrder(3)]
    public Guid CategoryId { get; set; }
    [JsonPropertyOrder(4)]
    public string? CategoryName { get; set; }

    [JsonPropertyOrder(5)]
    public string? WaerHousId { get; set; }
    [JsonPropertyOrder(6)]
    public string? WaerHousName { get; set; }
    [JsonPropertyOrder(7)]
    public DateTime? ExpirationDate { get; set; }

    // Prices
    [JsonPropertyOrder(8)]
    public decimal PurchasePrice { get; set; }
    [JsonPropertyOrder(9)]
    public decimal SalePrice { get; set; }
    [JsonPropertyOrder(10)]
    public decimal DiscountPercentageFromSupplier { get; set; }
}
