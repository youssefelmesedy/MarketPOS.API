namespace MarketPOS.Infrastructure.Repositories.RepositorySupplier;
public class SupplierRepo : GenericeRepository<Supplier>, ISupplierRepo
{
    public SupplierRepo(ApplicationDbContext context) : base(context)
    {
    }
}
