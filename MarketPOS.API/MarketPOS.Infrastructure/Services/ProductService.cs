using MarketPOS.Application.Common.RepositoryInterfaces;
using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Application.Specifications;

namespace Market.POS.Infrastructure.Services;
public class ProductService : GenericService<Product>, IProductService
{
    private readonly IProductRepo _productRepository;
    public ProductService(IReadOnlyRepository<Product> query, IFullRepository<Product> write, IUnitOfWork unitOfWork, IStringLocalizer<GenericService<Product>> localizer, ILogger<ProductService> logger, IProductRepo productRepository) : base(query, write, unitOfWork, localizer, logger)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<Product>>GetAllWithCategoryAsync
        (Guid? CategoryId = null,
        List<Func<IQueryable<Product>, IQueryable<Product>>>? IncludeExpression = null,
        bool IncludeSofteDelete = false)
    {
        return await _unitOfWork.ProductRepo.FindAsync(p => p.CategoryId == CategoryId,
                                 includeExpressions: IncludeExpression, includeSoftDeleted: IncludeSofteDelete);
            
    }

    public async Task<IEnumerable<Product>> GetByNameAsync(string name, List<Func<IQueryable<Product>, IQueryable<Product>>> includes, bool includeSoftDelete = false)
    {
        try
        {
            return await _unitOfWork.ProductRepo.FindAsync(p => p.Name.ToLower().Trim() == name.ToLower().Trim(), includeExpressions: includes, includeSoftDeleted: includeSoftDelete);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByNameFailed"]);
            throw;
        }
    }

    public Task<IEnumerable<Product>?> GetProductbyCategoryIdSpce(ISpecification<Product> specification)
    {
        try
        {

            var result = _productRepository.GetBySpecificationAsync(specification);
            return result!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Spcification Not Found");
            throw;
        }
    }

    public Task<Product?> GetWithUnitProfilesAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
