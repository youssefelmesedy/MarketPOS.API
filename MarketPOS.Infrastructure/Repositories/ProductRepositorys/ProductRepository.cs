namespace MarketPOS.Infrastructure.Repositories.ProductRepositorys;
public class ProductRepository : GenericRepository<Product>, IProductRepo
{
    public ProductRepository(ApplicationDbContext context) : base(context) { }

    public async Task<Product?> GetWithPricesAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.ProductPrice)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<Product?> GetWithUnitProfilesAsync(Guid id)
    {
        return await _dbSet
            .Include(p => p.ProductUnitProfile)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAllWithCategoryAsync(Guid? categoryId = null)
    {
        var query = _dbSet.Include(p => p.Category).AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        return await query.ToListAsync();
    }
    
}
