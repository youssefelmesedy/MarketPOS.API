using MarketPOS.Application.RepositoryInterfaces.InterfaceGenerice;
namespace MarketPOS.Application.RepositoryInterfaces.ProductRepositorys;
public interface IProductUnitProfileRepo
    :IProjectableRepository<ProductUnitProfile>, IQueryableRepository<ProductUnitProfile>, IWritableRepository<ProductUnitProfile>
{
    Task<ProductUnitProfile> GetByProductIdAsync(Guid productId);
}
