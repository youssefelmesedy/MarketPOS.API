using MarketPOS.Shared.DTOs.Authentication;

namespace MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
public interface IAuthService
{
    Task<AuthDto> RegisterAsync(RegisterDto register, string folderName, CancellationToken cancellationToken = default);
    Task<RefreshTokenDto> LoginAsync(LoginDto login, CancellationToken cancellationToken = default);
    Task<RefreshTokenDto> RefreshTokenAsync(string token);
}
