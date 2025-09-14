using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Services.InterfacesServices;
public interface IAggregateService
{
    IProductService ProductService { get; }
    ICategoryService CategoryService { get; }
    IProductPriceService ProductPriceService { get; }
    IProductUnitProfileService ProductUnitProfileService { get; }
    IActiveingredinentService ActiveingredinentService { get; }
    IWareHouseService HouseService { get; }
    ISupplierService SupplierService { get; }
}
