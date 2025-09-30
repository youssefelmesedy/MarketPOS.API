using MarketPOS.Shared.DTOs.Authentication;

namespace MarketPOS.Application.Features.CQRS.CQRSAuth.Command;
public class LoginCommand : IRequest<ResultDto<RefreshTokenDto>>
{
    public LoginDto dto { get; set; }

    public LoginCommand(LoginDto dto)
    {
        this.dto = dto;
    }
}
