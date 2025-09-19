using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.ActivelngredientsDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Query.QueryHandler;
public class GetAllActiveIngredinentQueryHandler : BaseHandler<GetAllActiveIngredinentQueryHandler>,
    IRequestHandler<GetAllActiveIngredinentQuery, ResultDto<IEnumerable<ActiveIngredinentsDetalisDTO>>>
{
    public GetAllActiveIngredinentQueryHandler(
        IServiceFactory services,
        IResultFactory<GetAllActiveIngredinentQueryHandler> resultFactory,
        IMapper? mapper = null,
        IStringLocalizer<GetAllActiveIngredinentQueryHandler>? localizer = null,
        ILocalizationPostProcessor localizationPostProcessor = null!)
        : base(services, resultFactory, mapper, null, localizer, localizationPostProcessor)
    {
    }

    public async Task<ResultDto<IEnumerable<ActiveIngredinentsDetalisDTO>>> Handle(GetAllActiveIngredinentQuery request, CancellationToken cancellationToken)
    {
        var _service = _servicesFactory.GetService<IActiveingredinentService>();

        var entitys = await _service.GetAllAsync(includeSoftDeleted: request.SoftDelete);
        if (!entitys.Any())
            return _resultFactory.Fail<IEnumerable<ActiveIngredinentsDetalisDTO>>("GetAllFailed");

        var result = _mapper?.Map<IEnumerable<ActiveIngredinentsDetalisDTO>>(entitys);
        if (result is null)
            return _resultFactory.Fail<IEnumerable<ActiveIngredinentsDetalisDTO>>("MappingFiled");

        var reusltLocalizer = _localizationPostProcessor.Apply(result);

        return _resultFactory.Success(reusltLocalizer, "Success");
    }
}
