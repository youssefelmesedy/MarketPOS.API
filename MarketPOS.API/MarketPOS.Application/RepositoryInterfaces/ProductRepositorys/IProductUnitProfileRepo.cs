namespace MarketPOS.Application.RepositoryInterfaces.ProductRepositorys;
public interface IProductUnitProfileRepo: IFullRepository<ProductUnitProfile>
{
    Task<ProductUnitProfile> GetByProductIdAsync(Guid productId);
}
