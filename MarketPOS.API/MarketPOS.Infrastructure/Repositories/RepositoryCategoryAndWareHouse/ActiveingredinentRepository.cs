using MarketPOS.Application.RepositoryInterfaces.RepositoryCategoryAndWareHouse;

namespace MarketPOS.Infrastructure.Repositories.RepositoryCategoryAndWareHouse;
public class ActiveingredinentRepository : GenericeRepository<ActiveIngredinents>, IActivelngredinentsRepo
{
    public ActiveingredinentRepository(ApplicationDbContext context) : base(context)
    {
    }
}
