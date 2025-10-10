using MarketPOS.Infrastructure.Context.Persistence;
using MarketPOS.Infrastructure.Repositories.GenericRepositoryAndBaseBuliderQuery;

namespace MarketPOS.Infrastructure.Repositories.ProductRepositorys;
public class ProductUnitProfileRepository : GenericRepository<ProductUnitProfile>, IProductUnitProfileRepo
{
    private readonly ILogger<ProductUnitProfileRepository> _logger;
    public ProductUnitProfileRepository(ApplicationDbContext context, ILogger<ProductUnitProfileRepository> logger) : base(context, logger)
    {
        _logger = logger;
    }

    public async Task<ProductUnitProfile> GetByProductIdAsync(Guid productId)
    {
        var entity = await _dbSet.Include(p => p.Product)
            .FirstOrDefaultAsync(p => p.ProductId == productId);

        return entity ?? throw new KeyNotFoundException($"ProductUnitProfile with Product ID {productId} not found.");
    }
}
