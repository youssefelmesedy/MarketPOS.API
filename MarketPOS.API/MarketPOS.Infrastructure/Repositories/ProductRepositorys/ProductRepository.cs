using MarketPOS.Application.RepositoryInterfaces.ProductRepositorys;
using MarketPOS.Application.Specifications;

namespace MarketPOS.Infrastructure.Repositories.ProductRepositorys;
public class ProductRepository : GenericeRepository<Product>, IProductRepo
{
    private readonly ISpecificationEvaluator<Product> _specificationEvaluator;
    public ProductRepository(ApplicationDbContext context, ISpecificationEvaluator<Product> specificationEvaluator = null!) : base(context)
    {
        _specificationEvaluator = specificationEvaluator;
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
