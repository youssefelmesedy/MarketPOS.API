using Market.Domain.Entitys.PurchaseInvoices;

namespace Market.Domain.Entitys.Suppliers;

public class Supplier : BaseEntity
{
    public string Name { get; set; } = default!;
    public ContactInfo? ContactInfo { get; set; }

    public ICollection<PurchaseInvoice> PurchaseInvoices { get; set; } = new List<PurchaseInvoice>();
}

