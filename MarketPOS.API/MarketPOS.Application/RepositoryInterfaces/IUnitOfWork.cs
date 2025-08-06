using MarketPOS.Application.RepositoryInterfaces.RepositoryCategoryAndWareHouse;

namespace MarketPOS.Application.RepositoryInterfaces;
public interface IUnitOfWork
{
    IProductRepo ProductRepo { get; }
    ICategoryRepo CategoryRepo { get; }
    IProductPriceRepo ProductPriceRepo { get; }
    IProductUnitProfileRepo ProductUnitProfileRepo { get; }
    IWareHouseRepo WareHouseRepo { get; }
    ISupplierRepo SupplierRepo { get; }

    Task<int> SaveChangesAsync();

}
