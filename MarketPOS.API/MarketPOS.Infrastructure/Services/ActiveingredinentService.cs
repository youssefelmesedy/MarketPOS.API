using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Infrastructure.Services;
public class ActiveingredinentService : GenericService<ActiveIngredinents>, IActiveingredinentService
{
    public ActiveingredinentService(
        IUnitOfWork unitOfWork, 
        IStringLocalizer<GenericService<ActiveIngredinents>> localizer,
        ILogger<ActiveingredinentService> logger)
        : base(unitOfWork, localizer, logger)
    {
    }
}
