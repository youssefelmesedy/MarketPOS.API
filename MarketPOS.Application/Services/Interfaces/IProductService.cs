namespace Market.POS.Application.Services.Interfaces;
public interface IProductService :
    IQueryableService<Product>,
    IProjectableService<Product>,
    IWritableService<Product>
{
    Task<IEnumerable<Product>> GetByNameAsync(string name, List<Func<IQueryable<Product>, IQueryable<Product>>> includes, bool icludSofteDelete = false);
}
