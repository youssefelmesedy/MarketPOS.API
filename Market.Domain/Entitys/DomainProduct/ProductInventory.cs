namespace Market.Domain.Entitys.DomainProduct;

// ProductInventory.cs
public class ProductInventory : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }

    public int Quantity { get; set; }

    public Product Product { get; set; } = default!;
    public Warehouse Warehouse { get; set; } = default!;
}

