using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;
using MarketPOS.Shared.DTOs.SofteDleteAndRestor;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Command.CommandHandler;
public class RestorIngredinentCommandHandler : BaseHandler<RestorIngredinentCommandHandler>,
    IRequestHandler<RestorIngredinentCommand, ResultDto<RestorDto>>
{
    public RestorIngredinentCommandHandler(
        IServiceFactory services,
        IResultFactory<RestorIngredinentCommandHandler> resultFactory,
        IMapper? mapper = null,
        IStringLocalizer<RestorIngredinentCommandHandler>? localizer = null) 
        : base(services, resultFactory, mapper, null, localizer)
    {
    }

    public async Task<ResultDto<RestorDto>> Handle(RestorIngredinentCommand request, CancellationToken cancellationToken)
    {
        var service = _servicesFactory.GetService<IActiveingredinentService>();

        var ingerdient = await service.GetByIdAsync(request.Id, includeSoftDeleted: true);
        if (ingerdient is null)
            return _resultFactory.Fail<RestorDto>("GetByIdFalied");

        if(!ingerdient.IsDeleted)
            return _resultFactory.Fail<RestorDto>("RestoreFailed");

         await service.RestoreAsync(ingerdient);

        var mapping = _mapper?.Map<RestorDto>(ingerdient);
        if (mapping is null)
            return _resultFactory.Fail<RestorDto>("MappingFailed");

        return _resultFactory.Success(mapping, "RestoredBy");
    }
}
