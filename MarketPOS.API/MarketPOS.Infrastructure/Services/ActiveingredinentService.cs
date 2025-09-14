using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Infrastructure.Services;
public class ActiveingredinentService : GenericService<ActiveIngredients>, IActiveingredinentService
{
    public ActiveingredinentService(
        IUnitOfWork unitOfWork, 
        IStringLocalizer<GenericService<ActiveIngredients>> localizer,
        ILogger<ActiveingredinentService> logger)
        : base(unitOfWork, localizer, logger)
    {
    }
}
