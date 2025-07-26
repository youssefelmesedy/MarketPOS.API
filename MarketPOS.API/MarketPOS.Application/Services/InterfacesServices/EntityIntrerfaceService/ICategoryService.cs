using MarketPOS.Application.Services.InterfacesServices.GenericeInterface;

namespace MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
public interface ICategoryService :
    IQueryableService<Category>,
    IProjectableService<Category>,
    IWritableService<Category>
{
    Task<IEnumerable<Category>> GetByNameAsync(string name, bool includeSofteDelete = false);
}
