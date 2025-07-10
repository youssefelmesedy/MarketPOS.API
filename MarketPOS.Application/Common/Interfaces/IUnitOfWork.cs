namespace MarketPOS.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IProductRepo ProductRepo { get; }
    ICategoryRepo CategoryRepo { get; }

    ISupplierRepo SupplierRepo { get; }
    //ICategoryRepo CategoryRepo { get; }

    Task<int> SaveChangesAsync();

}
