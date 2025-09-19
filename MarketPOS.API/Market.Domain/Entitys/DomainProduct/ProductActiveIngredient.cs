namespace Market.Domain.Entitys.DomainProduct;
public class ProductActiveIngredient
{
    public Guid ProductId { get; set; }
    public Product Product { get; set; } = default!;
    public Guid ActiveIngredinentsId { get; set; }
    public ActiveIngredients ActiveIngredinents { get; set; } = default!;

}
