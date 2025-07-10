using Market.Domain.Entitys.Suppliers;
using Market.POS.Infrastructure.Repositories;
using MarketPOS.Application.Common.Interfaces.RepositorySupplier;
using MarketPOS.Infrastructure.Context;

namespace MarketPOS.Infrastructure.Repositories.RepositorySupplier;

public class SupplierRepo : GenericRepository<Supplier>, ISupplierRepo
{
    public SupplierRepo(ApplicationDbContext context) : base(context)
    {
    }
}
