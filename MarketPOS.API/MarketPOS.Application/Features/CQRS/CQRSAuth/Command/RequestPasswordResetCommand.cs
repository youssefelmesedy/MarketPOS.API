using MarketPOS.Shared.DTOs.Authentication;
using MarketPOS.Shared.DTOs.AuthenticationDTO;

namespace MarketPOS.Application.Features.CQRS.CQRSAuth.Command;
public record RequestPasswordResetCommand : IRequest<ResultDto<AuthDto>>
{
    public RequestPasswordResetDto Dto { get; set; }

    public RequestPasswordResetCommand(RequestPasswordResetDto dto  )
    {
        Dto = dto;
    }
}
