using MarketPOS.Application.RepositoryInterfaces.RepositoryCategoryAndWareHouse;

namespace Market.POS.Infrastructure.Repositories;
public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IProductRepo ProductRepo{ get; }
    public ICategoryRepo CategoryRepo{ get; }
    public IProductPriceRepo ProductPriceRepo { get; } // Uncomment if you have a product price repository
    public IProductUnitProfileRepo ProductUnitProfileRepo { get; } // Uncomment if you have a product unit profile repository
    public ISupplierRepo SupplierRepo { get; } // Uncomment if you have a supplier repository

    public IWareHouseRepo WareHouseRepo { get; }

    public UnitOfWork(ApplicationDbContext context,
        IProductRepo productRepository,
        ICategoryRepo categoryRepository,
        ISupplierRepo supplierRepo,
        IProductPriceRepo productPriceRepo,
        IProductUnitProfileRepo porductUnitProfileRepo,
        IWareHouseRepo wareHouseRepo)
    {
        _context = context;
        ProductRepo = productRepository;
        CategoryRepo = categoryRepository;
        SupplierRepo = supplierRepo;
        ProductPriceRepo = productPriceRepo;
        ProductUnitProfileRepo = porductUnitProfileRepo;
        WareHouseRepo = wareHouseRepo;
    }


    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
