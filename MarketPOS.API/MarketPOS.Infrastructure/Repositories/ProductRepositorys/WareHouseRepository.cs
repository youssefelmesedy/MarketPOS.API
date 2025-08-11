using MarketPOS.Application.RepositoryInterfaces.RepositoryCategoryAndWareHouse;

namespace MarketPOS.Infrastructure.Repositories.ProductRepositorys;
public class WareHouseRepository : GenericeRepository<Warehouse>, IWareHouseRepo
{
    public WareHouseRepository(ApplicationDbContext context) : base(context)
    {
    }
}
