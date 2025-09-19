using MarketPOS.Infrastructure.Repositories.GenericRepositoryAndBaseBuliderQuery;

namespace MarketPOS.Infrastructure.Repositories.ProductRepositorys;
public class ProductRepository : GenericeRepository<Product>, IProductRepo
{
    public ProductRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Product>> GetAllWithCategoryAsync(Guid? categoryId = null)
    {
        var query = _dbSet.Include(p => p.Category).AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(p => p.CategoryId == categoryId.Value);

        return await query.ToListAsync();
    }
}
