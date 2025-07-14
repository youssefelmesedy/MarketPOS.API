namespace Market.Domain.Entitys.DomainProduct;

// ProductPrice.cs
public class ProductPrice : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;

    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal DiscountPercentageFromSupplier { get; set; }

    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }
    public bool IsActive { get; set; } = true;

    public ProductPrice() { }

    public ProductPrice(decimal salePrice, decimal? discountPercentageFromSupplier, decimal? purchasePrice)
    {
        SalePrice = salePrice;

        if (discountPercentageFromSupplier.HasValue && discountPercentageFromSupplier.Value > 0)
        {
            DiscountPercentageFromSupplier = discountPercentageFromSupplier.Value;
            PurchasePrice = CalculatePurchasePriceFromDiscount(salePrice, DiscountPercentageFromSupplier);
        }
        else if (purchasePrice.HasValue)
        {
            PurchasePrice = purchasePrice.Value;
            DiscountPercentageFromSupplier = CalculateDiscountFromPurchase(salePrice, PurchasePrice);
        }
        else
        {
            throw new ArgumentException("You must enter either the discount percentage or the purchase price.");
        }

        if (PurchasePrice > SalePrice)
            throw new ArgumentException("The purchase price cannot be greater than the selling price.");
    }

    public static decimal CalculatePurchasePriceFromDiscount(decimal salePrice, decimal discountPercentage)
        => Math.Round(salePrice * (1 - discountPercentage / 100), 2);

    public static decimal CalculateDiscountFromPurchase(decimal salePrice, decimal purchasePrice)
        => Math.Round((1 - (purchasePrice / salePrice)) * 100, 2);
}

