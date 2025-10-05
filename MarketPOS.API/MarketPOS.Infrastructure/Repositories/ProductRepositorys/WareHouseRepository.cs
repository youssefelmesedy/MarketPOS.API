using MarketPOS.Application.RepositoryInterfaces.RepositoryCategoryAndWareHouse;
using MarketPOS.Infrastructure.Repositories.GenericRepositoryAndBaseBuliderQuery;

namespace MarketPOS.Infrastructure.Repositories.ProductRepositorys;
public class WareHouseRepository : GenericRepository<Warehouse>, IWareHouseRepo
{
    private readonly ILogger<WareHouseRepository> _logger;
    public WareHouseRepository(ApplicationDbContext context, ILogger<WareHouseRepository> logger) : base(context, logger)
    {
        _logger = logger;
    }
}
