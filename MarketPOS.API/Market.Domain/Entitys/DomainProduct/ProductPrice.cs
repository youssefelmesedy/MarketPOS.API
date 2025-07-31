using System.ComponentModel.DataAnnotations;

namespace Market.Domain.Entitys.DomainProduct;

// ProductPrice.cs
public class ProductPrice : BaseEntity
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;

    public decimal PurchasePrice { get; set; }
    public decimal SalePrice { get; set; }
    public decimal DiscountPercentageFromSupplier { get; set; }

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

    public bool UpdateValues(decimal? newSalePrice, decimal? newPurchasePrice, decimal? newDiscountPercentage, string updatedBy)
    {
        bool isModified = false;

        if (newSalePrice.HasValue &&
            (!newPurchasePrice.HasValue || newPurchasePrice <= 0) &&
            (!newDiscountPercentage.HasValue || newDiscountPercentage <= 0))
        {
            throw new ValidationException("You must provide either a valid purchase price or a valid discount percentage when updating the sale price.");
        }

        if (!newSalePrice.HasValue && newPurchasePrice.HasValue && newPurchasePrice > SalePrice)
            throw new ValidationException("Purchase price cannot be greater than sale price.");

        if (newSalePrice.HasValue && newPurchasePrice.HasValue)
        {
            if (newPurchasePrice >= newSalePrice)
                throw new ValidationException("Purchase price cannot be greater than or equal to sale price.");

            if (!newDiscountPercentage.HasValue || newDiscountPercentage == 0)
            {
                var calculatedDiscount = CalculateDiscountFromPurchase(newSalePrice.Value, newPurchasePrice.Value);

                if (calculatedDiscount < 5m || calculatedDiscount > 100)
                    throw new ValidationException("Discount percentage cannot exceed 90% of sale price.");

                DiscountPercentageFromSupplier = calculatedDiscount;
                PurchasePrice = newPurchasePrice.Value;
                isModified = true;
            }
        }

        if (newDiscountPercentage.HasValue && newDiscountPercentage > 0)
        {
            if (newDiscountPercentage > 90)
                throw new ValidationException("Discount percentage cannot exceed 90% of sale price.");

            if (!newSalePrice.HasValue)
                throw new ValidationException("Sale price is required to calculate purchase price from discount.");

            var newCalculatedPurchase = CalculatePurchasePriceFromDiscount(newSalePrice.Value, newDiscountPercentage.Value);

            if (newCalculatedPurchase >= newSalePrice)
                throw new ValidationException("Calculated purchase price cannot be greater than or equal to sale price.");

            if (newCalculatedPurchase != PurchasePrice || newDiscountPercentage.Value != DiscountPercentageFromSupplier)
            {
                PurchasePrice = newCalculatedPurchase;
                DiscountPercentageFromSupplier = newDiscountPercentage.Value;
                isModified = true;
            }
        }

        else if (newPurchasePrice.HasValue && newPurchasePrice != PurchasePrice)
        {
            if (newPurchasePrice >= newSalePrice)
                throw new ValidationException("Purchase price cannot be greater than or equal to sale price.");

            var calculatedDiscount = CalculateDiscountFromPurchase(newSalePrice.Value, newPurchasePrice.Value);

            if (calculatedDiscount > 90)
                throw new ValidationException("Calculated discount percentage cannot exceed 90%.");

            PurchasePrice = newPurchasePrice.Value;
            DiscountPercentageFromSupplier = calculatedDiscount;
            isModified = true;
        }

        if (newSalePrice.HasValue && newSalePrice.Value != SalePrice)
        {
            if (newSalePrice <= PurchasePrice)
                throw new ValidationException("Sale price must be greater than purchase price.");

            SalePrice = newSalePrice.Value;
            isModified = true;
        }

        if (isModified)
        {
            UpdatedAt = DateTime.Now;
            ModifiedBy = updatedBy;
        }

        return isModified;
    }

    public static decimal CalculatePurchasePriceFromDiscount(decimal salePrice, decimal discountPercentage)
        => Math.Round(salePrice * (1 - discountPercentage / 100), 2);

    public static decimal CalculateDiscountFromPurchase(decimal salePrice, decimal purchasePrice)
        => Math.Round((1 - (purchasePrice / salePrice)) * 100, 2);

}

