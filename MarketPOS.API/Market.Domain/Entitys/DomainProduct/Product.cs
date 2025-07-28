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

    public void InitializeChildEntityinCreate()
    {
        CreatedAt = DateTime.Now;
        CreatedBy = "Yousse";

        ProductPrice.CreatedAt = DateTime.Now;
        ProductPrice.CreatedBy = "Youssef";

        ProductUnitProfile.CreatedAt = DateTime.Now;
        ProductUnitProfile.CreatedBy = "Youssef";

        IsDeleted = false;
    }
    public void InitializeChildEntityinUpdate()
    {
        UpdatedAt = DateTime.Now;
        ModifiedBy = "Youssef";
    }
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
}