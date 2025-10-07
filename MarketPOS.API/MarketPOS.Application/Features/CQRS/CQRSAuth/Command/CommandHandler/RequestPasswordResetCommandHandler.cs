using MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
using MarketPOS.Shared.Constants;
using MarketPOS.Shared.DTOs.Authentication;

namespace MarketPOS.Application.Features.CQRS.CQRSAuth.Command.CommandHandler;
public class RequestPasswordResetCommandHandler : BaseHandler<RequestPasswordResetCommandHandler>,
    IRequestHandler<RequestPasswordResetCommand, ResultDto<AuthDto>>
{
    public RequestPasswordResetCommandHandler(
        IServiceFactory serviceFactory,
        IResultFactory<RequestPasswordResetCommandHandler> resultFactory) : base(serviceFactory, resultFactory)
    {
    }

    public async Task<ResultDto<AuthDto>> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
    {
        var _authService = _servicesFactory.GetService<IAuthService>();

        var result = await _authService.RequestPasswordResetAsync(request.Dto, cancellationToken);
        if (result is null || !result.IsAuthenticated)
            return _resultFactory.Fail<AuthDto>(AppMessages.UserNotFound);

        return _resultFactory.Success(result, AppMessages.PasswordResetSuccessful);
    }
}
