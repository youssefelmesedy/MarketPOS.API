using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Infrastructure.Services;
public class WareHouseService : GenericService<Warehouse>, IWareHouseService
{
    public WareHouseService(
        IUnitOfWork unitOfWork, 
        IStringLocalizer<GenericService<Warehouse>> localizer,
        ILogger<WareHouseService> logger)
        : base(unitOfWork, localizer, logger)
    {
    }
}

