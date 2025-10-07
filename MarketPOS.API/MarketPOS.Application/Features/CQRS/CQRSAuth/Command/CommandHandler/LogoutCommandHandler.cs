
using MarketPOS.Application.Services.InterfacesServices;
using MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
using MarketPOS.Shared.Constants;

namespace MarketPOS.Application.Features.CQRS.CQRSAuth.Command.CommandHandler;
public class LogoutCommandHandler : BaseHandler<LogoutCommandHandler>,
    IRequestHandler<LogoutCommand, ResultDto<bool>>
{
    public LogoutCommandHandler(
        IServiceFactory serviceFactory,
        IResultFactory<LogoutCommandHandler> resultFactory) : base(serviceFactory, resultFactory)
    {
    }

    public async Task<ResultDto<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        var authService = _servicesFactory.GetService<IAuthService>();
        if (authService == null)
            return Fail<bool>(AppMessages.ArgumentNullException);

        var result = await authService.LogoutAsync(request.Token, cancellationToken);
        if (!result)
            return _resultFactory.Fail<bool>(AppMessages.LogoutFailed);

        return _resultFactory.Success(result, AppMessages.LogoutSuccessful);
    }
}
