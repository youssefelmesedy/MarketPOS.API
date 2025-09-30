using MarketPOS.Shared.DTOs.Authentication;

namespace MarketPOS.Application.Features.CQRS.CQRSAuth.Command;
public class RefreshTokenCommand : IRequest<ResultDto<RefreshTokenDto>>
{
    public string? RefreshToken { get; set; }
    public RefreshTokenCommand(string? refreshToken)
    {
        RefreshToken = refreshToken;
    }
}