using Market.POS.Application.Services.Interfaces;
using MarketPOS.Application.Common.Interfaces.ProductRepositorys;
using MarketPOS.Application.Common.Interfaces.RepositoryCategory;
using MarketPOS.Application.Common.Interfaces.RepositorySupplier;

namespace MarketPOS.Application.Common.Interfaces;

public interface IUnitOfWork
{
    IProductRepo ProductRepo { get; }
    ICategoryRepo CategoryRepo { get; }

    ISupplierRepo SupplierRepo { get; }
    //ICategoryRepo CategoryRepo { get; }

    Task<int> SaveChangesAsync();

}
