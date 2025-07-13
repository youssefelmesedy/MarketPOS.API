namespace MarketPOS.Infrastructure.Services;
public class CategoryService : GenericService<Category>, ICategoryService
{
    private readonly ILogger<CategoryService> _logger;
    private readonly IStringLocalizer<CategoryService> _localizer;

    public CategoryService(
        IQueryableRepository<Category> queryRepo,
        IProjectableRepository<Category> projectableRepo,
        IWritableRepository<Category> writableRepo,
        IUnitOfWork unitOfWork,
        ILogger<CategoryService> logger,
        IStringLocalizer<CategoryService> localizer)
        : base(unitOfWork, logger, queryRepo, projectableRepo, writableRepo, localizer)
    {
        _logger = logger;
        _localizer = localizer;
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
