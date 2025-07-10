namespace Market.Domain.Entitys.PurchaseInvoices;

public class PurchaseInvoice : BaseEntity
{
    public Guid SupplierId { get; set; }
    public Supplier Supplier { get; set; } = default!;

    public Guid WarehouseId { get; set; }
    public Warehouse Warehouse { get; set; } = default!;

    public DateTime InvoiceDate { get; set; } = DateTime.UtcNow;

    public decimal TotalAmount { get; set; } // مجموع كل items
    public string? Notes { get; set; }

    public ICollection<PurchaseInvoiceItem> Items { get; set; } = new List<PurchaseInvoiceItem>();

    public void CalculateTotal()
    {
        TotalAmount = Items.Sum(item => item.TotalPrice);
    }
}


