using MarketPOS.Infrastructure.Repositories.GenericRepositoryAndBaseBuliderQuery;

namespace MarketPOS.Infrastructure.Repositories.RepositorySupplier;
public class SupplierRepo : GenericRepository<Supplier>, ISupplierRepo
{
    private readonly ILogger<SupplierRepo> _logger;
    public SupplierRepo(ApplicationDbContext context, ILogger<SupplierRepo> logger) : base(context, logger)
    {
        _logger = logger;
    }
}
