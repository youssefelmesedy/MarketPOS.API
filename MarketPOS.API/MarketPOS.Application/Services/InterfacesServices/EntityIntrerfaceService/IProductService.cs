using MarketPOS.Application.Specifications;

namespace MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
public interface IProductService :IFullService<Product>, IReadOnlyService<Product>
{
    Task<IEnumerable<Product>> GetByNameAsync(string name, List<Func<IQueryable<Product>, IQueryable<Product>>> includes, bool icludSofteDelete = false);
    Task<Product?> GetWithUnitProfilesAsync(Guid id);
    Task<IEnumerable<Product>?> GetProductbyCategoryIdSpce(ISpecification<Product> specification);
    Task<IEnumerable<Product>> GetAllWithCategoryAsync
        (Guid? CategoryId = null,
        List<Func<IQueryable<Product>, IQueryable<Product>>>? IncludeExpression = null,
        bool IncludeSofteDelete = false);
}
