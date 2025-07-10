namespace MarketPOS.Infrastructure.Services;

public class CategoryService : GenericService<Category>, ICategoryService
{
    private readonly ILogger<CategoryService> _logger;
    public CategoryService(IGenericRepository<Category> repository, IUnitOfWork unitOfWork, ILogger<CategoryService> logger) : base(repository, unitOfWork, logger)
    {
        _logger = logger;
    }
    public async Task<IEnumerable<Category>> GetByNameAsync(string name)
    {
        try
        {
            return await FindAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting categories by name.");
            return Enumerable.Empty<Category>();
        }
    }
}
