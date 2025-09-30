namespace Market.Domain.Entitys.DomainCategory;
public class Category : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    public ICollection<Product> Products { get; set; } = new List<Product>();

    public override void InitializeChildEntityinCreate()
    {
        base.InitializeChildEntityinCreate();
    }

}

