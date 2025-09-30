using MarketPOS.Shared.Eunms;
using Microsoft.AspNetCore.Http;

namespace MarketPOS.Application.Services.InterfacesServices.FileStorage;
public interface IFileService
{
    Task<string> SaveUserImageAsync(Guid userId, string PersonFullName,IFormFile file, string folderName = "images", CancellationToken cancellationToken = default);
    void DeleteUserImage(Guid userId, string userName, string folderName);
}
