using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;

namespace MarketPOS.Application.Services.InterfacesServices;
public interface IAggregateService
{
    IAuthService AuthService { get; }
    IProductService ProductService { get; }
    ICategoryService CategoryService { get; }
    IProductPriceService ProductPriceService { get; }
    IProductUnitProfileService ProductUnitProfileService { get; }
    IActiveingredinentService ActiveingredinentService { get; }
    IWareHouseService HouseService { get; }
    ISupplierService SupplierService { get; }
}
