namespace MarketPOS.Application.Services.Interfaces;
public interface ICategoryService : IGenericService<Category>
{
    Task<IEnumerable<Category>> GetByNameAsync(string name);
}
