namespace MarketPOS.Infrastructure.Services;

public class SupplierService : GenericService<Supplier>, ISupplierService
{
    public SupplierService(IGenericRepository<Supplier> repository, IUnitOfWork unitOfWork, ILogger<SupplierService> logger)
        : base(repository, unitOfWork, logger)
    {
    }
}

