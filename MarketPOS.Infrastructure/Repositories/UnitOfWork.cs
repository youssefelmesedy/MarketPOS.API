using MarketPOS.Application.Common.Interfaces;
using MarketPOS.Application.Common.Interfaces.ProductRepositorys;
using MarketPOS.Application.Common.Interfaces.RepositoryCategory;
using MarketPOS.Application.Common.Interfaces.RepositorySupplier;
using MarketPOS.Infrastructure.Context;

namespace Market.POS.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public IProductRepo ProductRepo{ get; }
    public ICategoryRepo CategoryRepo{ get; }
    public ISupplierRepo SupplierRepo { get; } // Uncomment if you have a supplier repository

    public UnitOfWork(ApplicationDbContext context,
        IProductRepo productRepository,
        ICategoryRepo categoryRepository,
        ISupplierRepo supplierRepo)
    {
        _context = context;
        ProductRepo = productRepository;
        CategoryRepo = categoryRepository;
        SupplierRepo = supplierRepo;
    }


    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
