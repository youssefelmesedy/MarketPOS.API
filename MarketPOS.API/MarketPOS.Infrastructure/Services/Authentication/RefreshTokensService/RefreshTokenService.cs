using Market.Domain.Entities.Auth;
using MarketPOS.Application.InterfaceCacheing;
using MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
using Microsoft.AspNetCore.Http;
using System.Runtime.CompilerServices;
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
            (Guid userId, string ipAddress, string device, int expiryDays = 15)
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
                ExpiresAt = DateTime.UtcNow.AddDays(expiryDays),
                CreatedAt = DateTime.UtcNow,
                Revoked = null,
                IpAddress = ipAddress,
                Device = device
            };

            await repo.AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

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

            if (refreshToken is null || refreshToken.Revoked != null || refreshToken.IsExpired)
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

            var rehreshToken = await repo.GetAsync(t => t.RefToken == token, tracking: true);

            if (rehreshToken is null || rehreshToken.Revoked != null)
            {
                _logger.LogWarning("Attempt to revoke an invalid or already revoked token: {RefToken}", rehreshToken?.RefToken);
                return false;
            }

            rehreshToken.Revoked = DateTime.UtcNow;
            repo.Update(rehreshToken);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Revoked refresh token: {RefToken}", rehreshToken.RefToken);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while revoking refresh With User Id: {token}", token);
            throw;
        }
    }

    public async Task<bool> CleanupOldTokensAsync(Guid userId, string device, int maxTokensPerDevice = 50)
    {
        try
        {
            var repo = _unitOfWork.RepositoryEntity<RefreshToken>();

            // ✅ هات كل التوكنات لنفس المستخدم والجهاز
            var tokens = await repo.FindAsync(t => t.UserId == userId && t.Device == device, tracking: true);

            if (tokens == null || tokens.Count() <= maxTokensPerDevice)
                return false;

            // ✅ رتبهم من الأحدث للأقدم
            var orderedTokens = tokens
                .OrderByDescending(t => t.CreatedAt)
                .ToList();

            // ✅ سيب آخر واحد نشط أو مستخدم
            var activeToken = orderedTokens.FirstOrDefault(t => t.Revoked == null && t.ExpiresAt > DateTime.UtcNow)
                ?? orderedTokens.First();

            // ✅ احذف الأقدم بعد الحد المسموح، مع الحفاظ على النشط
            var tokensToDelete = orderedTokens
                .Where(t => t.Id != activeToken.Id)
                .Skip(maxTokensPerDevice - 1)
                .ToList();

            if (tokensToDelete.Any())
            {
                repo.RemoveRange(tokensToDelete);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("🧹 Cleaned {Count} old tokens for UserId: {UserId}, Device: {Device}",
                    tokensToDelete.Count, userId, device);
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error cleaning tokens for UserId: {UserId}, Device: {Device}", userId, device);
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

    public async Task<string> GetByTokenAsync(string token)
    {
        var repo = _unitOfWork.RepositoryEntity<RefreshToken>();
        var refreshToken = await repo.GetAsync(t => t.RefToken == token);
        if (refreshToken is null)
        {
            _logger.LogInformation("No refresh token found for the provided token.");
            return null!;
        }
        return refreshToken.RefToken ?? "Unknown";
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
