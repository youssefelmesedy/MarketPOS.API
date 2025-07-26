using MarketPOS.Application.Common.RepositoryInterfaces;
using MarketPOS.Application.Common.RepositoryInterfaces.InterfaceGenerice;
using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Infrastructure.Services;

public class ProductPriceService : GenericService<ProductPrice>, IProductPriceService
{
    private readonly IProductPriceRepo _productPriceRepository;

    public ProductPriceService(IReadOnlyRepository<ProductPrice> query, IFullRepository<ProductPrice> write, IUnitOfWork unitOfWork, IStringLocalizer<GenericService<ProductPrice>> localizer, ILogger<ProductPriceService> logger, IProductPriceRepo productPriceRepository) : base(query, write, unitOfWork, localizer, logger)
    {
        _productPriceRepository = productPriceRepository;
    }

    public async Task<ProductPrice> GetByProductIdAsync(Guid productId)
    {
        try
        {
            var result = await _productPriceRepository.GetByProductIdAsync(productId);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["GetByProductIdFailed"]);
            throw;
        }
    }

    public async Task<int> UpdateByProductIdAsync(ProductPrice productPrice)
    {
        try
        {
            var existing = await _productPriceRepository.GetByProductIdAsync(productPrice.ProductId);

            _write.Update(existing); // أو _productPriceRepository.Update(existing)
            await _unitOfWork.SaveChangesAsync();
            return 1;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, _localizer["UpdateFailed"]);
            throw;
        }
    }

}
