using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Infrastructure.Services;
public class WareHouseService : GenericService<Warehouse>, IWareHouseService
{
    public WareHouseService(IReadOnlyRepository<Warehouse> query, IFullRepository<Warehouse> write,
        IUnitOfWork unitOfWork, IStringLocalizer<GenericService<Warehouse>> localizer, ILogger<WareHouseService> logger)
        : base(query, write, unitOfWork, localizer, logger)
    {
    }
}

