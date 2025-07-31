using MarketPOS.Application.RepositoryInterfaces.InterfaceGenerice;

namespace MarketPOS.Application.RepositoryInterfaces.ProductRepositorys;
public interface IProductPriceRepo : IQueryableRepository<ProductPrice>,IWritableRepository<ProductPrice>
{
    Task<ProductPrice> GetByProductIdAsync(Guid Id);
}
