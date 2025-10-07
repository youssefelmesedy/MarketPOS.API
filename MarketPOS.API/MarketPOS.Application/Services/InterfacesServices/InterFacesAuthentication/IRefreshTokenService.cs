using Market.Domain.Entities.Auth;
using MarketPOS.Shared.DTOs.Authentication;
using Microsoft.AspNetCore.Http;

namespace MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
public interface IRefreshTokenService : IFullService<RefreshToken>
{
    Task<RefreshToken> LastRefreshToken(Guid userId);
    Task<RefreshToken> GenerateTokenAsync
            (Guid userId, string ipAddress, string device, int expiryDays = 15);
    Task<bool> ValidateTokenAsync(Guid userId);
    Task<bool> RevokeTokenAsync(string token);
    Task<bool> CleanupOldTokensAsync(Guid userId, string device, int maxTokens = 5);
    Task<string> GetByTokenAsync(string token);
    string GetIpAdress(HttpContext context);
    string GetDevice(HttpContext context);
}
