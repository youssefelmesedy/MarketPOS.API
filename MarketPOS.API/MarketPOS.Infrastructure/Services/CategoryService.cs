using MarketPOS.Application.Common.RepositoryInterfaces;
using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Infrastructure.Services;
public class CategoryService : GenericService<Category>, ICategoryService
{
    public CategoryService(IReadOnlyRepository<Category> query, IFullRepository<Category> write, IUnitOfWork unitOfWork, IStringLocalizer<GenericService<Category>> localizer, ILogger<CategoryService> logger) : base(query, write, unitOfWork, localizer, logger)
    {
    }

    public async Task<IEnumerable<Category>> GetByNameAsync(string name, bool includeSofteDelete = false)
    {
        try
        {
            return await FindAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim(), includeSoftDeleted: includeSofteDelete);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByNameFailed"]);
            throw;
        }
    }

}
