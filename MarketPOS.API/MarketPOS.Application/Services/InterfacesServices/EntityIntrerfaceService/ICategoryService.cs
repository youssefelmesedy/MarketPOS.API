namespace MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
public interface ICategoryService : IFullService<Category>, IReadOnlyService<Category>
{
    Task<IEnumerable<Category>> GetByNameAsync(string name, bool includeSofteDelete = false);
}
