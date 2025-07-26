using MarketPOS.Application.Common.RepositoryInterfaces.InterfaceGenerice;
using MarketPOS.Application.Specifications;

namespace MarketPOS.Application.Common.Interfaces.ProductRepositorys;
public interface IProductRepo : 
    IQueryableRepository<Product>,
    IProjectableRepository<Product>,
    IWritableRepository<Product>
{
    Task<Product?> GetWithPricesAsync(Guid id);
    Task<Product?> GetWithUnitProfilesAsync(Guid id);
    Task<IEnumerable<Product>> GetAllWithCategoryAsync(Guid? CategoryId = null);
    Task<IEnumerable<Product>> GetBySpecificationAsync(ISpecification<Product> specification);
}
