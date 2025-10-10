using MarketPOS.Infrastructure.Repositories.GenericRepositoryAndBaseBuliderQuery;

namespace MarketPOS.Infrastructure.Repositories.RepositoryCategory;
public class CategoryRepository : GenericRepository<Category>, ICategoryRepo
{
    public CategoryRepository(ApplicationDbContext context, ILogger<CategoryRepository> logger = null!) : base(context, logger)
    {
    }
}
