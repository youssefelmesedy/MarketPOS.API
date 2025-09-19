using MarketPOS.Infrastructure.Repositories.GenericRepositoryAndBaseBuliderQuery;

namespace MarketPOS.Infrastructure.Repositories.ProductRepositorys;
public class ProductUnitProfileRepository : GenericeRepository<ProductUnitProfile>, IProductUnitProfileRepo
{
    public ProductUnitProfileRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ProductUnitProfile> GetByProductIdAsync(Guid productId)
    {
        var entity = await _dbSet.Include(p => p.Product)
            .FirstOrDefaultAsync(p => p.ProductId == productId);

        return entity ?? throw new KeyNotFoundException($"ProductUnitProfile with Product ID {productId} not found.");
    }
}
