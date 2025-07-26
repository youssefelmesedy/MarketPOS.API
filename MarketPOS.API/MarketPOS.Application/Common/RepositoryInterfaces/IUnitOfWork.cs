namespace MarketPOS.Application.Common.Interfaces;
public interface IUnitOfWork
{
    IProductRepo ProductRepo { get; }
    ICategoryRepo CategoryRepo { get; }
    IProductPriceRepo ProductPriceRepo { get; }
    ISupplierRepo SupplierRepo { get; }

    Task<int> SaveChangesAsync();

}
