using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Application.Specifications;

namespace Market.POS.Infrastructure.Services;
public class ProductService : GenericService<Product>, IProductService
{
    public ProductService(
        IUnitOfWork unitOfWork, 
        IStringLocalizer<GenericService<Product>> localizer, 
        ILogger<ProductService> logger)
        : base(unitOfWork, localizer, logger)
    {
    }

    public async Task<IEnumerable<Product>>GetAllWithCategoryAsync
        (Guid? CategoryId = null,
        List<Func<IQueryable<Product>, IQueryable<Product>>>? IncludeExpression = null,
        bool IncludeSofteDelete = false)
    {
        return await _unitOfWork.Repository<IProductRepo>().FindAsync(p => p.CategoryId == CategoryId,
                                 includeExpressions: IncludeExpression, includeSoftDeleted: IncludeSofteDelete);
            
    }

    public async Task<IEnumerable<Product>> GetByNameAsync(string name, List<Func<IQueryable<Product>, IQueryable<Product>>> includes, bool includeSoftDelete = false)
    {
        try
        {
            return await _unitOfWork.Repository<IProductRepo>().FindAsync(p => p.Name.ToLower().Trim() == name.ToLower().Trim(), includeExpressions: includes, includeSoftDeleted: includeSoftDelete);
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

            var result = _unitOfWork.Repository<IProductRepo>().GetBySpecificationAsync(specification);
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
