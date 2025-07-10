namespace Market.Domain.Entitys.DomainProduct;
public class Product : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Barcode { get; set; }

    public Guid CategoryId { get; set; }
    public Category? Category { get; set; } = default!;

    public DateTime ExpirationDate { get; set; }

    // one to one 
    public ProductPrice ProductPrice { get; set; } = default!;
    public ProductUnitProfile ProductUnitProfile { get; set; } = default!;

    public ICollection<ProductInventory> ProductInventories { get; set; } = new List<ProductInventory>();

    public bool IsExpired() =>
        ExpirationDate < DateTime.UtcNow;
}


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


// ProductUnitProfile.cs
public class ProductUnitProfile : BaseEntity
{
    public Guid ProductId { get; set; }

    public string LargeUnitName { get; set; } = default!;
    public string MediumUnitName { get; set; } = default!;
    public string SmallUnitName { get; set; } = default!;

    public int MediumPerLarge { get; set; } = 1;
    public int SmallPerMedium { get; set; } = 1;

    public decimal LargeUnitPrice { get; set; }
    public decimal MediumUnitPrice { get; set; }
    public decimal SmallUnitPrice { get; set; }

    public Product Product { get; set; } = default!;

    public void CalculateUnitPricesFromLargeUnit()
    {
        if (MediumPerLarge <= 0 || SmallPerMedium <= 0)
            throw new InvalidOperationException("MediumPerLarge and SmallPerMedium must be greater than zero.");

        MediumUnitPrice = Math.Round(LargeUnitPrice / MediumPerLarge, 2);
        SmallUnitPrice = Math.Round(MediumUnitPrice / SmallPerMedium, 2);
    }
}


// Warehouse.cs
public class Warehouse : BaseEntity
{
    public string Name { get; set; } = default!;
    public ContactInfo? ContactInfo { get; set; }

    public ICollection<ProductInventory> ProductInventories { get; set; } = new List<ProductInventory>();
}


// ProductInventory.cs
public class ProductInventory : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }

    public int Quantity { get; set; }

    public Product Product { get; set; } = default!;
    public Warehouse Warehouse { get; set; } = default!;
}

