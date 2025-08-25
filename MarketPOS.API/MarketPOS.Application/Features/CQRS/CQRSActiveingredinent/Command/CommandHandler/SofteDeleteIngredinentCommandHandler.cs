using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Command.CommandHandler;
public class SofteDeleteIngredinentCommandHandler : BaseHandler<SofteDeleteIngredinentCommandHandler>,
    IRequestHandler<SofteDeleteIngredinentCommand, ResultDto<SofteDeleteDto>>
{
    public SofteDeleteIngredinentCommandHandler(
        IServiceFactory services,
        IResultFactory<SofteDeleteIngredinentCommandHandler> resultFactory,
        IMapper? mapper = null,
        IStringLocalizer<SofteDeleteIngredinentCommandHandler>? localizer = null,
        ILocalizationPostProcessor localizationPostProcessor = null!) 
        : base(services, resultFactory, mapper, null, localizer, localizationPostProcessor)
    {
    }

    public async Task<ResultDto<SofteDeleteDto>> Handle(SofteDeleteIngredinentCommand request, CancellationToken cancellationToken)
    {
        var service = _servicesFactory.GetService<IActiveingredinentService>();

        var ingredinent = await service.GetByIdAsync(request.Id);
        if (ingredinent is null)
            return _resultFactory.Fail<SofteDeleteDto>("GetByIdFailed");

        if(ingredinent.IsDeleted)
            return _resultFactory.Fail<SofteDeleteDto>("SofteDeletedFailed");

        var isDeletedIngredinent = await service.SoftDeleteAsync(ingredinent);

        var mapping = _mapper?.Map<SofteDeleteDto>(isDeletedIngredinent);
        if (mapping is null)
            return _resultFactory.Fail<SofteDeleteDto>("MappingFailed");

        var result = _localizationPostProcessor.Apply(mapping);

        return _resultFactory.Success(result, "Deleted");
    }
}
