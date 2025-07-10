namespace MarketPOS.Infrastructure.Repositories.RepositorySupplier;
public class SupplierRepo : GenericRepository<Supplier>, ISupplierRepo
{
    public SupplierRepo(ApplicationDbContext context) : base(context)
    {
    }
}
