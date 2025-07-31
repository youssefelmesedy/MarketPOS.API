using MarketPOS.Application.RepositoryInterfaces.RepositoryCategory;

namespace MarketPOS.Infrastructure.Repositories.RepositoryCategory;
public class CategoryRepository : GenericeRepository<Category>, ICategoryRepo
{
    public CategoryRepository(ApplicationDbContext context) : base(context)
    {
    }
}
