namespace MarketPOS.Shared.DTOs.ProductDto;
public class ProductDetailsDto  :BaseDto
{
    [JsonPropertyOrder(1)]
    public string Name { get; set; } = default!;
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
    public DateTime? ExpirationDate { get; set; }

    // Prices
    [JsonPropertyOrder(8)]
    public decimal PurchasePrice { get; set; }
    [JsonPropertyOrder(9)]
    public decimal SalePrice { get; set; }
    [JsonPropertyOrder(10)]
    public decimal DiscountPercentageFromSupplier { get; set; }

    // Units
    [JsonPropertyOrder(11)]
    public string LargeUnitName { get; set; } = default!;
    [JsonPropertyOrder(12)]
    public string MediumUnitName { get; set; } = default!;
    [JsonPropertyOrder(13)]
    public string SmallUnitName { get; set; } = default!;
    [JsonPropertyOrder(14)]
    public int MediumPerLarge { get; set; }
    [JsonPropertyOrder(15)]
    public int SmallPerMedium { get; set; }
    [JsonPropertyOrder(16)]
    public decimal LargeUnitPrice { get; set; }
    [JsonPropertyOrder(17)]
    public decimal MediumUnitPrice { get; set; }
    [JsonPropertyOrder(18)]
    public decimal SmallUnitPrice { get; set; }

    // Inventory Summary
    [JsonPropertyOrder(17)]
    public int TotalQuantityInStock { get; set; }
    [JsonPropertyOrder(18)]
    public string? WarehouseName { get; set; } = default!;
}
