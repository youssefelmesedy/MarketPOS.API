using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Query.QueryHandler;
public class GetByIdInegredinentQueryHandler : BaseHandler<GetAllActiveIngredinentQueryHandler>,
    IRequestHandler<GetByIdInegredinentQuery, ResultDto<ActiveIngredinentsDetalisDTO>>
{
    public GetByIdInegredinentQueryHandler(
        IServiceFactory services,
        IResultFactory<GetAllActiveIngredinentQueryHandler> resultFactory,
        IMapper? mapper = null,
        IStringLocalizer<GetAllActiveIngredinentQueryHandler>? localizer = null,
        ILocalizationPostProcessor localizationPostProcessor = null!)
        : base(services, resultFactory, mapper, null, localizer, localizationPostProcessor)
    {
    }

    public async Task<ResultDto<ActiveIngredinentsDetalisDTO>> Handle(GetByIdInegredinentQuery request, CancellationToken cancellationToken)
    {
        var servic = _servicesFactory.GetService<IActiveingredinentService>();

        var ingredinent = await servic.GetByIdAsync(request.Id, includeSoftDeleted: request.SofteDelete);
        if (ingredinent is null)
            return _resultFactory.Fail<ActiveIngredinentsDetalisDTO>("GetByIdFailed");

        var mapping = _mapper?.Map<ActiveIngredinentsDetalisDTO>(ingredinent);
        if (mapping is null)
            return _resultFactory.Fail<ActiveIngredinentsDetalisDTO>("MappingFailed");

        var result = _localizationPostProcessor.Apply(mapping);

        return _resultFactory.Success(result, "Success");
    }
}
