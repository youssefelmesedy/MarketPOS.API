using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Infrastructure.Services.AggregateService;
public class AggregateService : IAggregateService
{
    public IProductService ProductService { get; }
    public ICategoryService CategoryService { get; }
    public IProductPriceService ProductPriceService { get; }
    public IProductUnitProfileService ProductUnitProfileService { get; }
    public IActiveingredinentService ActiveingredinentService { get; }
    public IWareHouseService HouseService { get; }
    public ISupplierService SupplierService { get; }

    public AggregateService(
        IProductService productService,
        ICategoryService categoryService,
        IProductPriceService priceService,
        IProductUnitProfileService unitProfileService,
        IActiveingredinentService activeingredinent,
        IWareHouseService houseService,
        ISupplierService supplierService)
    {
        ProductService = productService;
        CategoryService = categoryService;
        ProductPriceService = priceService;
        ProductUnitProfileService = unitProfileService;
        ActiveingredinentService = activeingredinent;
        HouseService = houseService;
        SupplierService = supplierService;
    }
}

