using Microsoft.EntityFrameworkCore;

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

    public async Task<int> UpdateByProductIdAsync(ProductPrice productPrice)
    {
        var rowsAffected = await _dbSet
                .Where(p => p.ProductId == productPrice.ProductId)
                .ExecuteUpdateAsync(set => set
                .SetProperty(p => p.PurchasePrice, productPrice.PurchasePrice)
                .SetProperty(p => p.SalePrice, productPrice.SalePrice)
                .SetProperty(p => p.DiscountPercentageFromSupplier, productPrice.DiscountPercentageFromSupplier)
                .SetProperty(p => p.UpdatedAt, productPrice.UpdatedAt)
                .SetProperty(p => p.ModifiedBy, productPrice.ModifiedBy));

        if (rowsAffected == 0)
            throw new KeyNotFoundException($"ProductPrice for Product ID {productPrice.ProductId} not found.");

        return rowsAffected;
    }
}

