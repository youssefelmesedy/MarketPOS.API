using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Command.CommandHandler;
public class UpdateIngredinentCommandHandler : BaseHandler<UpdateIngredinentCommandHandler>,
             IRequestHandler<UpdateIngredinentCommand, ResultDto<Guid>>
{
    public UpdateIngredinentCommandHandler(
        IServiceFactory services,
        IResultFactory<UpdateIngredinentCommandHandler> resultFactory,
        IMapper? mapper = null,
        IStringLocalizer<UpdateIngredinentCommandHandler>? localizer = null)
        : base(services, resultFactory, mapper, null, localizer, null!)
    {
    }

    public async Task<ResultDto<Guid>> Handle(UpdateIngredinentCommand request, CancellationToken cancellationToken)
    {
        var _service = _servicesFactory.GetService<IActiveingredinentService>();

        var existIngredinent = await _service.GetByIdAsync(request.Id);
        if (existIngredinent is null)
            return _resultFactory.Fail<Guid>("GetByIdFailed");

        var existName = await _service.FindAsync(i => i.Name!.Trim().ToLower() == request.Dto.Name.Trim().ToLower()
                                                            && i.Id != request.Id);
        if(existName.Any())
            return _resultFactory.Fail<Guid>("DuplicateActiveIngredinentName");

        _mapper?.Map(request.Dto, existIngredinent);
        if (existIngredinent is null)
            return _resultFactory.Fail<Guid>("Mappingfailed");

        await _service.UpdateAsync(existIngredinent);

        return _resultFactory.Success(existIngredinent.Id, "Updated");
    }
}
