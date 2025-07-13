using Microsoft.EntityFrameworkCore.Query;

namespace Market.POS.Infrastructure.Services;
public class ProductService : GenericService<Product>, IProductService
{
    private readonly ILogger<ProductService> _logger;
    private readonly IStringLocalizer<ProductService> _localizer;

    public ProductService(
        IQueryableRepository<Product> queryRepo,
        IProjectableRepository<Product> projectableRepo,
        IWritableRepository<Product> writableRepo,
        IUnitOfWork unitOfWork,
        ILogger<ProductService> logger,
        IStringLocalizer<ProductService> localizer)
        : base(unitOfWork, logger, queryRepo, projectableRepo, writableRepo, localizer)
    {
        _logger = logger;
        _localizer = localizer;
    }
    public async Task<IEnumerable<Product>> GetByNameAsync(string name, List<Func<IQueryable<Product>, IQueryable<Product>>> includes, bool includeSoftDelete = false)
    {
        try
        {
            return await FindAsync(p => p.Name.ToLower().Trim() == name.ToLower().Trim(), includeExpressions: includes, includeSoftDeleted: includeSoftDelete);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByNameFailed"]);
            throw;
        }
    }

}
