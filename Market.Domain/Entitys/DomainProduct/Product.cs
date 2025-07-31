namespace Market.Domain.Entitys.DomainProduct;
public class Product : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Barcode { get; set; }
    public string RestoreBy { get; set; } = string.Empty;
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

        IsDeleted = false;

        if (ProductInventories != null)
        {
            foreach (var inventory in ProductInventories)
            {
                inventory.CreatedAt = DateTime.Now;
                inventory.CreatedBy = "Youssef";
            }
        }

        if (ProductPrice != null)
        {
            ProductPrice.CreatedAt = DateTime.Now;
            ProductPrice.CreatedBy = "Youssef";
            ProductPrice.ValidTo = DateTime.Now;
            ProductPrice.ValidFrom = DateTime.Now.AddDays(365);
        }

        if (ProductUnitProfile != null)
        {
            ProductUnitProfile.CreatedAt = DateTime.Now;
            ProductUnitProfile.CreatedBy = "Youssef";
        }
    }
    public void InitializeChildEntityinUpdate()
    {
        UpdatedAt = DateTime.Now;
        ModifiedBy = "Youssef";
    }
}