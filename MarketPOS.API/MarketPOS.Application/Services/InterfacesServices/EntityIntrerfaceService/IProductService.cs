namespace MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
public interface IProductService : IFullService<Product>
{
    Task<IEnumerable<Product>> GetByNameAsync(string name, List<Func<IQueryable<Product>, IQueryable<Product>>> includes, bool icludSofteDelete = false);

    Task<IEnumerable<Product>> GetAllWithCategoryAsync
        (Guid? CategoryId = null,
        List<Func<IQueryable<Product>, IQueryable<Product>>>? IncludeExpression = null,
        bool IncludeSofteDelete = false);

    Task UpdateProductIngredientAsync(Product product, List<Guid> ingredients);

    Task<List<Guid>> GetIngredientIdsByProductIdAsync(Guid productId);
}
