namespace Market.POS.Infrastructure.Services;
public class ProductService : GenericService<Product>, IProductService
{
    private ILogger<ProductService> _logger;
    public ProductService(IGenericRepository<Product> repository, IUnitOfWork unitOfWork, ILogger<ProductService> logger) : base(repository, unitOfWork, logger)
    {
        _logger = logger;
    }

    public async Task<IEnumerable<Product>> GetByNameAsync(string name, List<Func<IQueryable<Product>, IQueryable<Product>>> includes)
    {
        try
        {
            return await FindAsync(c => c.Name.ToLower().Trim() == name.ToLower().Trim(), false, includes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting categories by name.");
            return Enumerable.Empty<Product>();
        }
    }
}
