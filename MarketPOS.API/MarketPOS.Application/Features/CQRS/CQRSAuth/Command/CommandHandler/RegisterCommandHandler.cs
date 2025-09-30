using MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
using MarketPOS.Shared.DTOs.Authentication;

namespace MarketPOS.Application.Features.CQRS.CQRSAuth.Command.CommandHandler;
public class RegisterCommandHandler : BaseHandler<RegisterCommandHandler>,
    IRequestHandler<RegisterCommand, ResultDto<AuthDto>>
{
    public RegisterCommandHandler(
        IServiceFactory serviceFactory,
        IResultFactory<RegisterCommandHandler> resultFactory,
        IMapper mapper, 
        IStringLocalizer<RegisterCommandHandler> stringLocalizer) 
        : base(serviceFactory, resultFactory, mapper, null, stringLocalizer)
    {
    }
    public async Task<ResultDto<AuthDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var authService = _servicesFactory.GetService<IAuthService>();

        var register = await authService.RegisterAsync(request.dto, request.folderName);
        if(register is null)
            return _resultFactory.Fail<AuthDto>($"Message: {register!.Message}");

        if (!register.IsAuthenticated)
            return _resultFactory.Fail<AuthDto>($"Message: {register.Message}");

        return _resultFactory.Success(register, "Registration Successfully");
    }
}
