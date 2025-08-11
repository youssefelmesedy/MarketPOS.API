namespace MarketPOS.Infrastructure.Repositories.ProductRepositorys;
public class ProductPriceRepository : GenericeRepository<ProductPrice>, IProductPriceRepo
{
    public ProductPriceRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ProductPrice> GetByProductIdAsync(Guid productId)
    {
        var entity = await _dbSet
            .Include(p => p.Product)
            .FirstOrDefaultAsync(p => p.ProductId == productId); 

        return entity ?? throw new KeyNotFoundException($"ProductPrice with Product ID {productId} not found.");
    }
}

