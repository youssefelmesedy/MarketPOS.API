namespace Market.Domain.Entitys.DomainCategory;
public class Category : BaseEntity
{
    public string Name { get; set; } = default!;
    public string? Description { get; set; }

    

    public ICollection<Product> Products { get; set; } = new List<Product>();

    public void InitializeBasePropertyInCreate()
    {
        CreatedAt = DateTime.Now;
        CreatedBy = "Youssef";

        IsDeleted = false;
    }

    public void InitializeBaseInUpDate()
    {
        UpdatedAt = DateTime.Now;
        ModifiedBy = "Youssef";


    }
}

