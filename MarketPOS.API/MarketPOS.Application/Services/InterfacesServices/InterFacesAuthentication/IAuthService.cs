using MarketPOS.Shared.DTOs.Authentication;
using MarketPOS.Shared.DTOs.AuthenticationDTO;
using Microsoft.AspNetCore.Mvc;

namespace MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
public interface IAuthService
{
    // --- Auth Core ---
    Task<AuthDto> RegisterAsync(RegisterDto register, string folderName, CancellationToken cancellationToken = default);
    Task<RefreshTokenDto> LoginAsync(LoginDto login, CancellationToken cancellationToken = default);
    Task<RefreshTokenDto> RefreshTokenAsync(string token);
    Task<bool> LogoutAsync(string token, CancellationToken cancellationToken = default);

    // --- Password Management ---
    Task<AuthDto> ChangePasswordAsync(Guid userId, ChangePasswordDto dto, CancellationToken cancellationToken = default);
    Task<AuthDto> RequestPasswordResetAsync(RequestPasswordResetDto dto, CancellationToken cancellationToken = default);
    Task<AuthDto> ResetPasswordAsync(ResetPasswordDto dto, CancellationToken cancellationToken = default);

    // --- Email Verification ---
    Task<AuthDto> SendEmailVerificationAsync(Guid userId, CancellationToken cancellationToken = default);
    Task<AuthDto> VerifyEmailAsync(VerifyEmailDto dto, CancellationToken cancellationToken = default);
}

