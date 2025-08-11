using System.ComponentModel.DataAnnotations;

namespace Market.Domain.Entitys.DomainCategory;

public class ActiveIngredinents : BaseEntity
{
    [Required]
    public string? Name { get; set; }

   public ICollection<Product> products { get; set; } = new List<Product>();

    public override void InitializeChildEntityinCreate()
    {
        base.InitializeChildEntityinCreate();
    }
}
