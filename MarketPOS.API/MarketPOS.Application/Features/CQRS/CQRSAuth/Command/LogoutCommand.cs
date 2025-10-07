namespace MarketPOS.Application.Features.CQRS.CQRSAuth.Command;
public class LogoutCommand : IRequest<ResultDto<bool>>
{
    public string Token { get; set; }

    public LogoutCommand(string token)
    {
        Token = token;
    }
}
