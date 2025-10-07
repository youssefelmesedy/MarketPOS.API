using MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
using MarketPOS.Shared.DTOs.Authentication;

namespace MarketPOS.Application.Features.CQRS.CQRSAuth.Command.CommandHandler;
public class RefreshTokenCommandHanndler : BaseHandler<RefreshTokenCommandHanndler>,
    IRequestHandler<RefreshTokenCommand, ResultDto<RefreshTokenDto>>
{
    public RefreshTokenCommandHanndler(
        IServiceFactory serviceFactory,
        IResultFactory<RefreshTokenCommandHanndler> resultFactory) 
        : base(serviceFactory, resultFactory)
    {
    }
    public async Task<ResultDto<RefreshTokenDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var _authService = _servicesFactory.GetService<IAuthService>() 
            ?? throw new InvalidOperationException("IAuthService not registered in the service factory.");
        
        var auth = await _authService.RefreshTokenAsync(request.RefreshToken!);
        if (auth is null || !auth.IsActive)
            return _resultFactory.Fail<RefreshTokenDto>($"{auth!.Message}");

        return _resultFactory.Success(auth, "Token refreshed successfully.");
    }
}
