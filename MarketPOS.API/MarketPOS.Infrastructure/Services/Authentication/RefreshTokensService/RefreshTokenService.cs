using Market.Domain.Entities.Auth;
using MarketPOS.Application.InterfaceCacheing;
using MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
using Microsoft.AspNetCore.Http;
using System.Security.Cryptography;

public class RefreshTokenService : GenericServiceCacheing<RefreshToken>, IRefreshTokenService
{

    public RefreshTokenService(IUnitOfWork unitOfWork,
        IStringLocalizer<RefreshTokenService> localizer,
        ILogger<RefreshTokenService> logger,
        IGenericCache cache)
        : base(unitOfWork, localizer, logger, cache)
    {
    }

    public async Task<RefreshToken> GenerateTokenAsync
            (Guid userId, string ipAddress, string device, int expiryDays = 1, int maxTokens = 5)
    {
        try
        {
            var repo = _unitOfWork.RepositoryEntity<RefreshToken>();
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            var token = Convert.ToBase64String(randomNumber);

            var refreshToken = new RefreshToken
            {
                RefToken = token,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.ToLocalTime().AddDays(expiryDays),
                CreatedAt = DateTime.UtcNow.ToLocalTime(),
                IsRevoked = null,
                IpAddress = ipAddress,
                Device = device
            };

            await repo.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            await CleanupOldTokensAsync(userId, maxTokens);

            _logger.LogInformation("Generated new refresh token for UserId: {UserId}", userId);
            return refreshToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while generating refresh token for UserId: {UserId}", userId);
            throw;
        }
    }

    public async Task<bool> ValidateTokenAsync(Guid userId)
    {
        try
        {
            var repo = _unitOfWork.RepositoryEntity<RefreshToken>();
            var refreshToken = await repo.GetAsync(t => t.UserId == userId);

            if (refreshToken is null || refreshToken.IsRevoked != null || refreshToken.IsExpired)
            {
                _logger.LogWarning("Invalid or expired refresh token: {RefToken}", refreshToken?.RefToken);
                return false;
            }

            _logger.LogInformation("Refresh token is valid: {RefToken}", refreshToken.RefToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while validating refresh token with UserId: {userId}", userId);
            throw;
        }
    }

    public async Task<bool> RevokeTokenAsync(string token)
    {
        try
        {
            var repo = _unitOfWork.RepositoryEntity<RefreshToken>();
            var refreshToken = await repo.GetAsync(t => t.RefToken == token);

            if (refreshToken is null || refreshToken.IsRevoked != null)
            {
                _logger.LogWarning("Attempt to revoke an invalid or already revoked token: {RefToken}", refreshToken?.RefToken);
                return false;
            }

            refreshToken.IsRevoked = DateTime.UtcNow.ToLocalTime();
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Revoked refresh token: {RefToken}", refreshToken.RefToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while revoking refresh With User Id: {token}", token);
            throw;
        }
    }

    public async Task<bool> CleanupOldTokensAsync(Guid userId, int maxTokens = 5)
    {
        try
        {
            var repo = _unitOfWork.RepositoryEntity<RefreshToken>();

            var tokens = await repo.GetAllAsync();
            if (tokens.Count(t => t.UserId == userId && t.IsRevoked != null) <= maxTokens)
                return false;

            var oldTokens = tokens
                .OrderByDescending(t => t.CreatedAt)
                .Skip(maxTokens)
                .ToList();

            if (!oldTokens.Any())
                return false;

            repo.RemoveRange(oldTokens);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Cleaned up {Count} old tokens for UserId: {UserId}", oldTokens.Count, userId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while cleaning up old tokens for UserId: {UserId}", userId);
            throw;
        }
    }

    public async Task<RefreshToken> LastRefreshToken(Guid userId)
    {
        var repo = _unitOfWork.RepositoryEntity<RefreshToken>();
        var tokens = (await repo.FindAsync(t => t.UserId == userId))
                     .OrderByDescending(c => c.CreatedAt).FirstOrDefault();
        if (tokens is null)
        {
            _logger.LogInformation("No refresh tokens found in the database.");
            return null!;
        }

        return tokens;
    }

    public string GetIpAdress(HttpContext context)
    {
        if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
            return context.Request.Headers["X-Forwarded-For"].ToString() ?? "Unknown";

        return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }

    public string GetDevice(HttpContext context)
    {
        return context.Request.Headers["User-Agent"].ToString() ?? "Unknown";
    }
}
