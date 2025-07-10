namespace MarketPOS.Infrastructure.Repositories.RepositoryCategory
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepo
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
