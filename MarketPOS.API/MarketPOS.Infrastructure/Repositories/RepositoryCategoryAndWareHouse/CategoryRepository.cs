using MarketPOS.Infrastructure.Repositories.GenericRepositoryAndBaseBuliderQuery;

namespace MarketPOS.Infrastructure.Repositories.RepositoryCategory;
public class CategoryRepository : GenericRepository<Category>, ICategoryRepo
{
    private readonly ILogger<CategoryRepository>? _logger;
    public CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger) : base(context, logger)
    {
    }
}
