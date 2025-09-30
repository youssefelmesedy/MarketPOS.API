using MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
using MarketPOS.Shared.DTOs.Authentication;

namespace MarketPOS.Application.Features.CQRS.CQRSAuth.Command.CommandHandler;
public class LoginCommandHandler : BaseHandler<LoginCommandHandler>,
    IRequestHandler<LoginCommand, ResultDto<RefreshTokenDto>>
{
    public LoginCommandHandler(
        IServiceFactory serviceFactory,
        IResultFactory<LoginCommandHandler> resultFactory)
        : base(serviceFactory, resultFactory)
    {}
    public async Task<ResultDto<RefreshTokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var _authService = _servicesFactory.GetService<IAuthService>() ?? throw new InvalidOperationException("IAuthService not login in the service factory.");

        var authDto = await  _authService.LoginAsync(request.dto);
        if(authDto is null)
            return _resultFactory.Fail<RefreshTokenDto>($"Message: {authDto!.Message}");

        return _resultFactory.Success(authDto, $"Login Successfully");
    }
}
