namespace MarketPOS.Shared.DTOs.ProductDto;
public class CreateProductDto 
{
    public string Name { get; set; } = default!;
    public string? Barcode { get; set; }
    public List<Guid>? IngredinentId { get; set; }
    public Guid CategoryId { get; set; }

    // Price-related
    public decimal? PurchasePrice { get; set; }
    public decimal SalePrice { get; set; } // هذا هو LargeUnitPrice فعليًا
    public decimal? DiscountPercentageFromSupplier { get; set; }

    // Expiration
    public DateTime? ExpirationDate { get; set; }

    // Units
    public string LargeUnitName { get; set; } = default!;
    public string MediumUnitName { get; set; } = default!;
    public string SmallUnitName { get; set; } = default!;
    public int MediumPerLarge { get; set; }
    public int SmallPerMedium { get; set; }
}

