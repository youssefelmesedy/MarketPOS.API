using System;

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
    public ICollection<ProductActiveIngredient> ProductIngredients { get; set; } = new List<ProductActiveIngredient>();

    public bool IsExpired() =>
        ExpirationDate < DateTime.UtcNow;

    public bool UpdateValues(string? newName, string? newBarcode, Guid newCategoryId, DateTime newExpirationDate)
    {
        bool modified = false;

        if (!string.Equals(Name?.Trim(), newName?.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            Name = newName?.Trim()!;
            modified = true;
        }

        if (!string.Equals(Barcode?.Trim(), newBarcode?.Trim(), StringComparison.OrdinalIgnoreCase))
        {
            Barcode = newBarcode?.Trim();
            modified = true;
        }

        if (CategoryId != newCategoryId)
        {
            CategoryId = newCategoryId;
            modified = true;
        }

        if (ExpirationDate != newExpirationDate)
        {
            ExpirationDate = newExpirationDate;
            modified = true;
        }

        return modified;
    }

    public override void InitializeChildEntityinCreate()
    {
        base.InitializeChildEntityinCreate();

        ProductUnitProfile?.InitializeChildEntityinCreate();
        ProductPrice.InitializeChildEntityinCreate();
    }
}