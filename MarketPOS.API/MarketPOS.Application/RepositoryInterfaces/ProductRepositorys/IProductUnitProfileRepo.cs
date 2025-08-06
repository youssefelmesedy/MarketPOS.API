using MarketPOS.Application.RepositoryInterfaces.InterfaceGenerice;
namespace MarketPOS.Application.RepositoryInterfaces.ProductRepositorys;
public interface IProductUnitProfileRepo
    :IFullRepository<ProductUnitProfile>,IReadOnlyRepository<ProductUnitProfile>
{
    Task<ProductUnitProfile> GetByProductIdAsync(Guid productId);
}
