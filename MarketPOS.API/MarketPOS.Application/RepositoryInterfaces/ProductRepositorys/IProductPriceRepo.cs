namespace MarketPOS.Application.RepositoryInterfaces.ProductRepositorys;
public interface IProductPriceRepo : IFullRepository<ProductPrice>
{
    Task<ProductPrice> GetByProductIdAsync(Guid Id);
}
