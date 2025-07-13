namespace MarketPOS.Application.Common.Interfaces.ProductRepositorys;
public interface IProductRepo : 
    IQueryableRepository<Product>,
    IProjectableRepository<Product>,
    IWritableRepository<Product>
{
    Task<Product?> GetWithPricesAsync(Guid id);
    Task<Product?> GetWithUnitProfilesAsync(Guid id);
    Task<IEnumerable<Product>> GetAllWithCategoryAsync(Guid? CategoryId = null);
}
