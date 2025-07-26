using MarketPOS.Application.Specifications;

namespace MarketPOS.Infrastructure.Repositories.ProductRepositorys;
public class ProductRepository : GenericeRepository<Product>, IProductRepo
{
    private readonly ISpecificationEvaluator<Product> _specificationEvaluator;
    public ProductRepository(ApplicationDbContext context, ISpecificationEvaluator<Product> specificationEvaluator = null) : base(context)
    {
        _specificationEvaluator = specificationEvaluator;
    }

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
    public async Task<IEnumerable<Product>> GetBySpecificationAsync(ISpecification<Product> specification)
    {
        var query = _specificationEvaluator.GetQuery(_dbSet.AsQueryable(), specification);
        return await query.ToListAsync();
    }

}
