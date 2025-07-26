using MarketPOS.Application.Common.RepositoryInterfaces.InterfaceGenerice;

namespace MarketPOS.Application.Common.Interfaces.ProductRepositorys;
public interface IProductPriceRepo : IQueryableRepository<ProductPrice>,IWritableRepository<ProductPrice>
{
    Task<ProductPrice> GetByProductIdAsync(Guid Id);
    Task<int> UpdateByProductIdAsync(ProductPrice productPrice);
}
