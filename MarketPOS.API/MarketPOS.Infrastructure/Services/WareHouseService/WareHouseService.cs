using MarketPOS.Application.InterfaceCacheing;

namespace MarketPOS.Infrastructure.Services.WareHouseService;
public class WareHouseService : GenericServiceCacheing<Warehouse>, IWareHouseService
{
    public WareHouseService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<GenericService<Warehouse>> localizer,
        ILogger<WareHouseService> logger, IGenericCache cache)
        : base(unitOfWork, localizer, logger, cache)
    {
    }
}

