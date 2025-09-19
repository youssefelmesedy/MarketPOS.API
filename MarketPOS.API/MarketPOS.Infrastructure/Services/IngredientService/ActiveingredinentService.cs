using MarketPOS.Application.InterfaceCacheing;

namespace MarketPOS.Infrastructure.Services.IngredientService;
public class ActiveingredinentService : GenericServiceCacheing<ActiveIngredients>, IActiveingredinentService
{
    public ActiveingredinentService(
        IUnitOfWork unitOfWork,
        IStringLocalizer<ActiveingredinentService> stringLocalizer,
        ILogger<ActiveingredinentService> logger,
        IGenericCache cacheService)
        : base(unitOfWork, stringLocalizer, logger, cacheService)
    {
    }
}

