namespace Market.POS.Application.Services.Interfaces;
public interface IProductService : IGenericService<Product>
{
    Task<IEnumerable<Product>> GetByNameAsync(string name, List<Func<IQueryable<Product>, IQueryable<Product>>> includes);
}
