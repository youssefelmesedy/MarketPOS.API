using Market.Domain.Entities.Auth;
using MarketPOS.Application.InterfaceCacheing;
using MarketPOS.Application.Services.InterfacesServices.FileStorage;
using MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
using MarketPOS.Shared.DTOs.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace MarketPOS.Infrastructure.Services.Authentication.AuthenticationService;

public class AuthService : IAuthService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IFileService _fileService;
    private readonly IJwtService _jwtService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<AuthService> _logger;
    private readonly IStringLocalizer<AuthService> _localizer;
    private readonly IGenericCache _cache;
    private readonly string _cacheKeyPrefix;

    public AuthService(
        IUnitOfWork unitOfWork,
        IJwtService jwtService,
        IRefreshTokenService refreshTokenService,
        IHttpContextAccessor httpContextAccessor,
        ILogger<AuthService> logger,
        UserManager<User> userManager,
        RoleManager<IdentityRole<Guid>> roleManager,
        IFileService fileService,
        IStringLocalizer<AuthService> localizer,
        IGenericCache cache)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
        _fileService = fileService ?? throw new ArgumentNullException(nameof(fileService));
        _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
        _refreshTokenService = refreshTokenService ?? throw new ArgumentNullException(nameof(refreshTokenService));
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _localizer = localizer ?? throw new ArgumentNullException(nameof(localizer));
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        _cacheKeyPrefix = typeof(AuthService).Name;
    }
    public async Task<AuthDto> RegisterAsync(RegisterDto register, string foldername, CancellationToken cancellationToken = default)
    {
        if (register == null)
            throw new ArgumentNullException(nameof(register));

        return await _unitOfWork.ExecuteInTransactionAsync(async cancellationToken =>
        {
            if (await _userManager.FindByEmailAsync(register.Email) != null)
                return new AuthDto { Message = _localizer["EmailAlreadyRegistered"] };

            if (await _userManager.FindByNameAsync(register.UserName) != null)
                return new AuthDto { Message = _localizer["UsernameAlreadyTaken"] };

            if (register.File == null || register.File.Length == 0)
                return new AuthDto { Message = _localizer["ProfileImageRequired"] };

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserName = register.UserName,
                Email = register.Email,
                CreatedAt = DateTime.UtcNow
            };

            user.ProfileImageUrl = await _fileService.SaveUserImageAsync(
                user.Id,
                user.UserName.Trim(),
                register.File,
                foldername
            );

            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthDto { Message = $"{_localizer["RegistrationFailed"]}: {errors}" };
            }

            // ✅ Add Default Role
            var defaultRole = "User";
            if (!await _roleManager.RoleExistsAsync(defaultRole))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>(defaultRole));
            }
            await _userManager.AddToRoleAsync(user, defaultRole);

            // ✅ Get roles + cache them
            var cacheKey = _cache.BuildCacheKey("UserRoles", _cacheKeyPrefix, user.Id);
            var roles = await _cache.GetOrAddAsync(cacheKey, async () =>
            {
                var r = await _userManager.GetRolesAsync(user);
                return r.ToList();
            }, TimeSpan.FromMinutes(30));

            var accessToken = _jwtService.GenerateToken(user.Id, user.UserName!, user.Email!, roles);

            var context = _httpContextAccessor.HttpContext!;
            var refreshToken = await _refreshTokenService.GenerateTokenAsync(
                user.Id,
                _refreshTokenService.GetIpAdress(context),
                _refreshTokenService.GetDevice(context), 
                expiryDays: 15, 
                maxTokens: 5
            );

            _logger.LogInformation("User registered successfully: {Email}", register.Email);

            return new AuthDto
            {
                Message = _localizer["RegistrationSuccessful"],
                IsAuthenticated = true,
                FullName = user.FullName ?? "Unk",
                UserName = user.UserName ?? "Unk",
                Email = user.Email ?? "Unk",
                Roles = roles,
                Token = accessToken,
                ExpiresAt = refreshToken.ExpiresAt,
                RefreshToken = refreshToken.RefToken ?? string.Empty,
                ProfileImageURL = user.ProfileImageUrl ?? string.Empty
            };
        }, cancellationToken);
    }

    public async Task<RefreshTokenDto> LoginAsync(LoginDto login, CancellationToken cancellationToken = default)
    {
        var user = await _userManager.FindByEmailAsync(login.EmailORUserName)
                   ?? await _userManager.FindByNameAsync(login.EmailORUserName);

        if (user == null || !await _userManager.CheckPasswordAsync(user, login.Password))
        {
            _logger.LogInformation("Invalid credentials for {Input}", login.EmailORUserName);
            return new RefreshTokenDto { Message = _localizer["InvalidCredentials"] };
        }

        var cacheKey = _cache.BuildCacheKey("UserRoles", _cacheKeyPrefix, user.Id);

        var Roles = await _cache.GetOrAddAsync(cacheKey, async () =>
          {
              var roles = await _userManager.GetRolesAsync(user);
              return roles.ToList();
          }, TimeSpan.FromMinutes(30));

        var accessToken = _jwtService.GenerateToken(user.Id, user.UserName!, user.Email!, Roles);

        var context = _httpContextAccessor.HttpContext!;
        var repo = _unitOfWork.RepositoryEntity<RefreshToken>();
        var lastRefreshToken = await _refreshTokenService.LastRefreshToken(user.Id);

        RefreshToken newRefreshToken;

        if (lastRefreshToken != null)
        {
            await _refreshTokenService.RevokeTokenAsync(lastRefreshToken.RefToken!);
        }

        newRefreshToken = await _refreshTokenService.GenerateTokenAsync(
            user.Id,
            _refreshTokenService.GetIpAdress(context),
            _refreshTokenService.GetDevice(context),
            expiryDays: 15, maxTokens: 5);

        return new RefreshTokenDto
        {
            Message = _localizer["LoginSuccessful"],
            IsActive = true,
            FullName = user.FullName ?? "Unk",
            Token = accessToken,
            RefreshToken = newRefreshToken.RefToken ?? string.Empty,
            ExpiredAt = newRefreshToken.ExpiresAt
        };
    }

    public async Task<RefreshTokenDto> RefreshTokenAsync(string token)
    {
        var tokenDTO = new RefreshTokenDto();

        if (string.IsNullOrWhiteSpace(token))
        {
            _logger.LogInformation("Refresh token is empty or null");
            tokenDTO.Message = _localizer["InvalidToken"];
            return tokenDTO;
        }

        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.RefreshTokens!.Any(rt => rt.RefToken == token));

        if (user is null)
        {
            _logger.LogInformation("User not found for refresh token: {RefToken}", token);
            tokenDTO.Message = _localizer["UserNotFound"];
            return tokenDTO;
        }

        var refreshToken = user.RefreshTokens!.SingleOrDefault(rt => rt.RefToken == token);
        if (refreshToken is null || !refreshToken.IsActive)
        {
            _logger.LogInformation("Inactive or invalid refresh token: {RefToken}", token);
            tokenDTO.Message = _localizer["InactiveToken"];
            return tokenDTO;
        }

        await _refreshTokenService.RevokeTokenAsync(refreshToken.RefToken!);

        // ✅ Get roles from cache (instead of always DB)
        var cacheKey = _cache.BuildCacheKey("UserRoles", _cacheKeyPrefix, user.Id);
        var roles = await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            var r = await _userManager.GetRolesAsync(user);
            return r.ToList();
        }, TimeSpan.FromMinutes(30));

        var newAccessToken = _jwtService.GenerateToken(user.Id, user.UserName!, user.Email!, roles);

        var newRefreshToken = await _refreshTokenService.GenerateTokenAsync(
            user.Id,
            _refreshTokenService.GetIpAdress(_httpContextAccessor.HttpContext!),
            _refreshTokenService.GetDevice(_httpContextAccessor.HttpContext!),
            expiryDays: 15, maxTokens: 5);

        tokenDTO.Message = _localizer["TokenRefreshed"];
        tokenDTO.IsActive = true;
        tokenDTO.FullName = user.FullName ?? "Unk";
        tokenDTO.Token = newAccessToken;
        tokenDTO.RefreshToken = newRefreshToken.RefToken ?? "Unk";
        tokenDTO.ExpiredAt = newRefreshToken.ExpiresAt;

        _logger.LogInformation("Token refreshed successfully for user: {UserId}", user.Id);
        return tokenDTO;
    }
}

