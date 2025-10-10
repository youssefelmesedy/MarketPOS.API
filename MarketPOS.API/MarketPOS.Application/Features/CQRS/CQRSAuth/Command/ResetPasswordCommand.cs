using MarketPOS.Shared.DTOs.Authentication;
using MarketPOS.Shared.DTOs.AuthenticationDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MarketPOS.Application.Features.CQRS.CQRSAuth.Command
{
    public class ResetPasswordCommand : IRequest<ResultDto<AuthDto>>
    {
        public ResetPasswordDto Dto { get; set; }

        public ResetPasswordCommand(ResetPasswordDto dto)
        {
            Dto = dto;
        }
    }
}
