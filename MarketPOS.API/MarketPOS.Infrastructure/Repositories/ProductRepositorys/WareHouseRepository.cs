using MarketPOS.Application.RepositoryInterfaces.RepositoryCategoryAndWareHouse;
using MarketPOS.Infrastructure.Repositories.GenericRepositoryAndBaseBuliderQuery;

namespace MarketPOS.Infrastructure.Repositories.ProductRepositorys;
public class WareHouseRepository : GenericeRepository<Warehouse>, IWareHouseRepo
{
    public WareHouseRepository(ApplicationDbContext context) : base(context)
    {
    }
}
