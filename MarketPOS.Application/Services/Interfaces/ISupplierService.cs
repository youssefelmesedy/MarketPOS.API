namespace MarketPOS.Application.Services.Interfaces;
public interface ISupplierService :
    IQueryableService<Supplier>,
    IProjectableService<Supplier>,
    IWritableService<Supplier>
{
}
