namespace MarketPOS.Application.Common.Interfaces.ProductRepositorys;

public interface IProductRepo : IGenericRepository<Product>
{
    Task<Product?> GetWithPricesAsync(Guid id);
    Task<Product?> GetWithUnitProfilesAsync(Guid id);
    Task<IEnumerable<Product>> GetAllWithCategoryAsync(Guid? CategoryId = null);
}
