using MarketPOS.Application.RepositoryInterfaces.RepositoryCategoryAndWareHouse;

namespace MarketPOS.Infrastructure.Repositories.RepositoryCategoryAndWareHouse;
public class WareHouseRepository : GenericeRepository<Warehouse>, IWareHouseRepo
{
    public WareHouseRepository(ApplicationDbContext context) : base(context)
    {
    }
}
