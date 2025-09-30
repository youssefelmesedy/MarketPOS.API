using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Market.Domain.Entities.Auth;
namespace MarketPOS.Infrastructure.Context;
public class ApplicationDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }
    public DbSet<RefreshToken> RefreshTokens { get; set; } = default!;

    public DbSet<Product> Products { get; set; } = default!;
    public DbSet<ProductPrice> ProductPrices { get; set; } = default!;
    public DbSet<ProductUnitProfile> ProductUnitProfiles { get; set; } = default!;
    public DbSet<Warehouse> Warehouses { get; set; } = default!;
    public DbSet<ProductInventory> ProductInventories { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Supplier> Suppliers { get; set; } = default!;
    public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; } = default!;
    public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; } = default!;
    public DbSet<ProductActiveIngredient> ProductActiveIngredients { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}
