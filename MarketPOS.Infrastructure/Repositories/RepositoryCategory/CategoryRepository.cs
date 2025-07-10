using Market.Domain.Entitys.DomainCategory;
using Market.POS.Infrastructure.Repositories;
using MarketPOS.Application.Common.Interfaces.RepositoryCategory;
using MarketPOS.Infrastructure.Context;

namespace MarketPOS.Infrastructure.Repositories.RepositoryCategory
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepo
    {
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
