using MarketPOS.Application.RepositoryInterfaces;
using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Infrastructure.Services;
public class SupplierService : GenericService<Supplier>, ISupplierService
{
    public SupplierService(IReadOnlyRepository<Supplier> query, IFullRepository<Supplier> write, IUnitOfWork unitOfWork, IStringLocalizer<GenericService<Supplier>> localizer, ILogger<SupplierService> logger) : base(query, write, unitOfWork, localizer, logger)
    {
    }
}


