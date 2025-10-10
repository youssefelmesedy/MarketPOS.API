using MarketPOS.Shared.DTOs.Authentication;

namespace MarketPOS.Application.Features.CQRS.CQRSAuth.Command;
public record RegisterCommand : IRequest<ResultDto<AuthDto>>
{
    public RegisterationDto dto { get; set; }
    public string folderName { get; set; }
    public RegisterCommand(RegisterationDto dto, string folderName)
    {
        this.dto = dto;
        this.folderName = folderName;
    }
}
