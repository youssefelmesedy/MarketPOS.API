namespace MarketPOS.Application.RepositoryInterfaces.ProductRepositorys;
public interface IProductRepo : IFullRepository<Product>
{
    Task<IEnumerable<Product>> GetAllWithCategoryAsync(Guid? CategoryId = null);
}
