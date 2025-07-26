using MarketPOS.Application.Common.RepositoryInterfaces.InterfaceGenerice;

namespace MarketPOS.Application.Common.Interfaces.RepositorySupplier;
public interface ISupplierRepo  : 
    IQueryableRepository<Supplier>,
    IProjectableRepository<Supplier>,
    IWritableRepository<Supplier>
{

}
