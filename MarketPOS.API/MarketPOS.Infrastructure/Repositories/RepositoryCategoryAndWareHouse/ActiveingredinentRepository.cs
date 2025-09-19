using MarketPOS.Application.RepositoryInterfaces.RepositoryCategoryAndWareHouse;
using MarketPOS.Infrastructure.Repositories.GenericRepositoryAndBaseBuliderQuery;

namespace MarketPOS.Infrastructure.Repositories.RepositoryCategoryAndWareHouse;
public class ActiveingredinentRepository : GenericeRepository<ActiveIngredients>, IActivelngredinentsRepo
{
    public ActiveingredinentRepository(ApplicationDbContext context) : base(context)
    {
    }
}
