using MarketPOS.Application.RepositoryInterfaces.InterfaceGenerice;

namespace MarketPOS.Application.RepositoryInterfaces.RepositorySupplier;
public interface ISupplierRepo  : 
    IQueryableRepository<Supplier>,
    IProjectableRepository<Supplier>,
    IWritableRepository<Supplier>
{

}
