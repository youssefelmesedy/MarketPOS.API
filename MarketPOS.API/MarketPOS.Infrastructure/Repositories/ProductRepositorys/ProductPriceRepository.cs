using MarketPOS.Infrastructure.Context.Persistence;
using MarketPOS.Infrastructure.Repositories.GenericRepositoryAndBaseBuliderQuery;

namespace MarketPOS.Infrastructure.Repositories.ProductRepositorys;
public class ProductPriceRepository : GenericRepository<ProductPrice>, IProductPriceRepo
{
    private readonly ILogger<ProductPriceRepository> _logger;
    public ProductPriceRepository(ApplicationDbContext context, ILogger<ProductPriceRepository> logger) : base(context, logger)
    {
        _logger = logger;
    }

    public async Task<ProductPrice> GetByProductIdAsync(Guid productId)
    {
        var entity = await _dbSet
            .Include(p => p.Product)
            .FirstOrDefaultAsync(p => p.ProductId == productId);

        return entity ?? throw new KeyNotFoundException($"ProductPrice with Product ID {productId} not found.");
    }
}

