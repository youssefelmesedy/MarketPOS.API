using MarketPOS.Application.InterfaceCacheing;

namespace MarketPOS.Infrastructure.Services.SupplierService;
public class SupplierService : GenericServiceCacheing<Supplier>, ISupplierService
{
    public SupplierService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<GenericService<Supplier>> localizer,
        ILogger<SupplierService> logger, IGenericCache cache)
        : base(unitOfWork, localizer, logger, cache)
    {
    }
}


