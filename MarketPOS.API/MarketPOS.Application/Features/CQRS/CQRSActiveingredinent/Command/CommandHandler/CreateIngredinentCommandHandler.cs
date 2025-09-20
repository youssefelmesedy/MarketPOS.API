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
        var ingredientService = _servicesFactory.GetService<IActiveingredinentService>();

        var newName = request.DTO.Name.Trim().ToLower();

        if (await ingredientService.AnyAsync(p => p.Name!.ToLower().Trim() == newName, true))
            return _resultFactory.Fail<Guid>($"DuplicateActiveIngredinentName");

        var mapping = _mapper?.Map<ActiveIngredients>(request.DTO);
        if (mapping is null)
            return _resultFactory.Fail<Guid>("Mappingfailed");

        await ingredientService.AddAsync(mapping);

        return _resultFactory.Success(mapping.Id, "Created");
    }
}
