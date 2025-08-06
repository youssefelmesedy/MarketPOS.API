using MarketPOS.Application.RepositoryInterfaces.InterfaceGenerice;
using MarketPOS.Application.Specifications;

namespace MarketPOS.Application.RepositoryInterfaces.ProductRepositorys;
public interface IProductRepo : IFullRepository<Product>, IReadOnlyRepository<Product>
{
    Task<IEnumerable<Product>> GetAllWithCategoryAsync(Guid? CategoryId = null);
    Task<IEnumerable<Product>> GetBySpecificationAsync(ISpecification<Product> specification);
}
