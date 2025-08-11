using MarketPOS.Application.RepositoryInterfaces;
using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Infrastructure.Services;
public class SupplierService : GenericService<Supplier>, ISupplierService
{
    public SupplierService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<GenericService<Supplier>> localizer,
        ILogger<SupplierService> logger) 
        : base(unitOfWork, localizer, logger)
    {
    }
}


