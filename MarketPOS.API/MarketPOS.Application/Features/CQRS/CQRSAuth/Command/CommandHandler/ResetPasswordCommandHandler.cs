using MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
using MarketPOS.Shared.Constants;
using MarketPOS.Shared.DTOs.Authentication;

namespace MarketPOS.Application.Features.CQRS.CQRSAuth.Command.CommandHandler;
public class ResetPasswordCommandHandler : BaseHandler<ResetPasswordCommandHandler>,
    IRequestHandler<ResetPasswordCommand, ResultDto<AuthDto>>
{
    public ResetPasswordCommandHandler(
        IServiceFactory serviceFactory,
        IResultFactory<ResetPasswordCommandHandler> resultFactory) : base(serviceFactory, resultFactory)
    {
    }
    public async Task<ResultDto<AuthDto>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        var authService = _servicesFactory.GetService<IAuthService>();

        var result = await authService.ResetPasswordAsync(request.Dto);
        if(!result.IsAuthenticated)
            return _resultFactory.Fail<AuthDto>(AppMessages.UserNotFound);

        return _resultFactory.Success(result, AppMessages.PasswordResetSuccessful);
    }
}
