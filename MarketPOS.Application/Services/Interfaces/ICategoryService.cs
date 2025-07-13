namespace MarketPOS.Application.Services.Interfaces;
public interface ICategoryService :
    IQueryableService<Category>,
    IProjectableService<Category>,
    IWritableService<Category>
{
    Task<IEnumerable<Category>> GetByNameAsync(string name, bool includeSofteDelete = false);
}
