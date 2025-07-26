using MarketPOS.Application.Services.InterfacesServices.GenericeInterface;

namespace MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
public interface ISupplierService :
    IQueryableService<Supplier>,
    IProjectableService<Supplier>,
    IWritableService<Supplier>
{
}
