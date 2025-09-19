using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Query.QueryHandler;
public class GetIngredinentByNameQueryHandler : BaseHandler<GetIngredinentByNameQueryHandler>,
    IRequestHandler<GetIngredinentByNameQuery, ResultDto<ActiveIngredinentsDetalisDTO>>
{
    public GetIngredinentByNameQueryHandler(
        IServiceFactory services,
        IResultFactory<GetIngredinentByNameQueryHandler> resultFactory,
        IMapper? mapper = null,
        IStringLocalizer<GetIngredinentByNameQueryHandler>? localizer = null,
        ILocalizationPostProcessor localizationPostProcessor = null!)
        : base(services, resultFactory, mapper, null, localizer, localizationPostProcessor)
    {
    }

    public async Task<ResultDto<ActiveIngredinentsDetalisDTO>> Handle(GetIngredinentByNameQuery request, CancellationToken cancellationToken)
    {
        var service = _servicesFactory.GetService<IActiveingredinentService>();

        var ingredinents = await service.FindAsync(i => i.Name!.Trim().ToLower() == request.Name.Trim().ToLower(),
                                                         includeSoftDeleted: request.SofteDeleted);
        if (!ingredinents.Any())
            return _resultFactory.Fail<ActiveIngredinentsDetalisDTO>("NotFound");

        var mapping = _mapper?.Map<ActiveIngredinentsDetalisDTO>(ingredinents.First());
        if (mapping is null)
            return _resultFactory.Fail<ActiveIngredinentsDetalisDTO>("Mappingfailed");

        var result = _localizationPostProcessor.Apply(mapping);

        return _resultFactory.Success(result, "Success");
    }
}
