namespace Market.Domain.Entitys.PurchaseInvoices;
public class PurchaseInvoiceItem : BaseEntity
{
    public Guid PurchaseInvoiceId { get; set; }
    public PurchaseInvoice PurchaseInvoice { get; set; } = default!;

    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;

    public string UnitName { get; set; } = default!; // كرتونة، شريط، قطعة
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public decimal TotalPrice => Math.Round(Quantity * UnitPrice, 2);
}


