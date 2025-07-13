namespace MarketPOS.Infrastructure.Services;
public class SupplierService : GenericService<Supplier>, ISupplierService
{
    private readonly ILogger<SupplierService> _logger;

    public SupplierService(
        IQueryableRepository<Supplier> queryRepo,
        IProjectableRepository<Supplier> projectableRepo,
        IWritableRepository<Supplier> writableRepo,
        IUnitOfWork unitOfWork,
        ILogger<SupplierService> logger,
        IStringLocalizer<GenericService<Supplier>> localizer)
        : base(unitOfWork, logger, queryRepo, projectableRepo, writableRepo, localizer)
    {
        _logger = logger;
    }
}


