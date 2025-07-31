using MarketPOS.Application.RepositoryInterfaces.ProductRepositorys;
using MarketPOS.Application.RepositoryInterfaces.RepositoryCategory;
using MarketPOS.Application.RepositoryInterfaces.RepositorySupplier;

namespace MarketPOS.Application.RepositoryInterfaces;
public interface IUnitOfWork
{
    IProductRepo ProductRepo { get; }
    ICategoryRepo CategoryRepo { get; }
    IProductPriceRepo ProductPriceRepo { get; }
    IProductUnitProfileRepo ProductUnitProfileRepo { get; }
    ISupplierRepo SupplierRepo { get; }

    Task<int> SaveChangesAsync();

}
