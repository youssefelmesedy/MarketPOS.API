using System.ComponentModel.DataAnnotations;

namespace Market.Domain.Entitys.DomainCategory;

public class ActiveIngredients : BaseEntity
{
    [Required]
    public string? Name { get; set; }

    public ICollection<ProductActiveIngredient> ProductIngredient { get; set; } = new List<ProductActiveIngredient>();

    public override void InitializeChildEntityinCreate()
    {
        base.InitializeChildEntityinCreate();
    }
}
