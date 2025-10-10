using MarketPOS.Application.RepositoryInterfaces.RepositoryCategoryAndWareHouse;
using MarketPOS.Infrastructure.Context.Persistence;
using MarketPOS.Infrastructure.Repositories.GenericRepositoryAndBaseBuliderQuery;

namespace MarketPOS.Infrastructure.Repositories.RepositoryCategoryAndWareHouse;
public class ActiveingredinentRepository : GenericRepository<ActiveIngredients>, IActivelngredinentsRepo
{
    private readonly ILogger<ActiveingredinentRepository> _logger;
    public ActiveingredinentRepository(ApplicationDbContext context, ILogger<ActiveingredinentRepository> logger) : base(context, logger)
    {
        _logger = logger;
    }
}
