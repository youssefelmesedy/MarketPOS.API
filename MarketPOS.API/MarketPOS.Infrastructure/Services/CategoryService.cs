using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Infrastructure.Services;
public class CategoryService : GenericService<Category>, ICategoryService
{
    public CategoryService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<GenericService<Category>> localizer,
        ILogger<CategoryService> logger)
        : base(unitOfWork, localizer, logger)
    {
    }

    public async Task<IEnumerable<Category>> GetByNameAsync(string name, bool includeSofteDelete = false)
    {
        try
        {
            return await _unitOfWork.Repository<ICategoryRepo>().FindAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim(), includeSoftDeleted: includeSofteDelete);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByNameFailed"]);
            throw;
        }
    }

}
