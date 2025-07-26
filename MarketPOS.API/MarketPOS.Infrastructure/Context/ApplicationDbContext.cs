namespace MarketPOS.Infrastructure.Context;
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
    {
    }

    public DbSet<Product> Products {get; set;} = default!;
    public DbSet<ProductPrice> ProductPrices { get; set; } = default!;
    public DbSet<ProductUnitProfile> ProductUnitProfiles { get; set; } = default!;
    public DbSet<Warehouse> Warehouses { get; set; } = default!;
    public DbSet<ProductInventory> ProductInventories { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Supplier> Suppliers { get; set; } = default!;
    public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; } = default!;
    public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; } = default!;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
