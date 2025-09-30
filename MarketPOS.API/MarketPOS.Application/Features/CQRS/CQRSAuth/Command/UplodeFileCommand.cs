using MarketPOS.Shared.Eunms;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace MarketPOS.Application.Features.CQRS.CQRSAuth.Command;
public record UplodeFileCommand : IRequest<ResultDto<string>>
{
    public Guid UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public IFormFile? File { get; set; }
    public string FolderName { get; set; }

    public UplodeFileCommand(Guid userId,string userName, IFormFile file, string folderName = "images")
    {
        UserId = userId;
        UserName = userName;
        File = file;
        FolderName = folderName;
    }
}
