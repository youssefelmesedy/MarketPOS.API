using MarketPOS.Application.Services.InterfacesServices.EntityIntrerfaceService;

namespace MarketPOS.Application.Features.CQRS.CQRSActiveingredinent.Command.CommandHandler;
public class CreateIngredinentCommandHandler : BaseHandler<CreateIngredinentCommandHandler>,
    IRequestHandler<CreateActivIngredinentCommand, ResultDto<Guid>>
{
    public CreateIngredinentCommandHandler(
        IServiceFactory services,
        IResultFactory<CreateIngredinentCommandHandler> resultFactory,
        IMapper? mapper = null,
        IStringLocalizer<CreateIngredinentCommandHandler>? localizer = null,
        ILocalizationPostProcessor localizationPostProcessor = null!) 
        : base(services, resultFactory, mapper, null, localizer, localizationPostProcessor)
    {
    }

    public async Task<ResultDto<Guid>> Handle(CreateActivIngredinentCommand request, CancellationToken cancellationToken)
    {
        var _service = _servicesFactory.GetService<IActiveingredinentService>();

        var existEntity = await _service.FindAsync(a => a.Name!.Trim().ToLower() == request.DTO.Name.Trim().ToLower());
        if (existEntity.Any())
            return _resultFactory.Fail<Guid>($"DuplicateActiveIngredinentName");

        var mapping = _mapper?.Map<ActiveIngredinents>(request.DTO);
        if (mapping is null)
            return _resultFactory.Fail<Guid>("MappingFiled");

        await _service.AddAsync(mapping);

        return _resultFactory.Success(mapping.Id, "Created");
    }
}
