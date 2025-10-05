using MarketPOS.Infrastructure.Repositories.GenericRepositoryAndBaseBuliderQuery;

namespace MarketPOS.Infrastructure.Repositories.ProductRepositorys;
public class ProductRepository : GenericRepository<Product>, IProductRepo
{
    private readonly ILogger<ProductRepository> _logger;
    public ProductRepository(ApplicationDbContext context, ILogger<ProductRepository> logger) : base(context, logger)
    {
        _logger = logger; 
    }

    public async Task<IEnumerable<Product>> GetAllWithCategoryAsync(Guid? categoryId = null)
    {
        var query = _dbSet.Include(p => p.Category).AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        return await query.ToListAsync();
    }
}
