using MarketPOS.Application.RepositoryInterfaces.InterfaceGenerice;

namespace MarketPOS.Application.RepositoryInterfaces.ProductRepositorys;
public interface IProductPriceRepo : IFullRepository<ProductPrice>, IReadOnlyRepository<ProductPrice>
{
    Task<ProductPrice> GetByProductIdAsync(Guid Id);
}
