namespace MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
public interface ICategoryService : IFullService<Category>
{
    Task<IEnumerable<Category>> GetByNameAsync(string name, bool includeSofteDelete = false);
}
