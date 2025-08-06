namespace Market.Domain.Entitys.DomainProduct;

// Warehouse.cs
public class Warehouse : BaseEntity
{
    public string Name { get; set; } = default!;
    public ContactInfo? ContactInfo { get; set; }

    public ICollection<ProductInventory> ProductInventories { get; set; } = new List<ProductInventory>();

    public override void InitializeChildEntityinCreate()
    {
        base.InitializeChildEntityinCreate();
    }
}

