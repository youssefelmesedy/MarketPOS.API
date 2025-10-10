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
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;
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
        IGenericCache cache,
        IEmailService emailService,
        IConfiguration configuration)
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
        _emailService = emailService ?? throw new ArgumentNullException(nameof(emailService));
        _cacheKeyPrefix = typeof(AuthService).Name;
        _configuration = configuration;
    }
    public async Task<AuthDto> RegisterAsync(RegisterationDto register, string foldername, CancellationToken cancellationToken = default)
    {
        if (register == null)
            throw new ArgumentNullException(nameof(register));

        return await _unitOfWork.ExecuteInTransactionAsync(async cancellationToken =>
        {
            if (await _userManager.FindByEmailAsync(register.Email) != null)
                return new AuthDto { Message = AppMessages.EmailAlreadyRegistered};

            if (await _userManager.FindByNameAsync(register.UserName) != null)
                return new AuthDto { Message = AppMessages.UsernameAlreadyTaken};

            if (register.File == null || register.File.Length == 0)
                return new AuthDto { Message = AppMessages.ProfileImageRequired};

            var user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = register.FirstName,
                LastName = register.LastName,
                UserName = register.UserName,
                Email = register.Email,
                Gmail = register.Gmail,
                CreatedAt = DateTime.UtcNow
            };

            // ✅ Save profile image safely
            try
            {
                user.ProfileImageUrl = await _fileService.SaveUserImageAsync(
                    user.Id,
                    user.UserName.Trim(),
                    register.File,
                    foldername
                );
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving profile image for {Email}", register.Email);
                return new AuthDto { Message = AppMessages.UplodeImageFilde };
            }

            // ✅ Create user
            var result = await _userManager.CreateAsync(user, register.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthDto { Message = $"{AppMessages.RegistrationFailed}: {errors}" };
            }

            // ✅ Assign default role
            const string defaultRole = "User";
            if (!await _roleManager.RoleExistsAsync(defaultRole))
            {
                await _roleManager.CreateAsync(new IdentityRole<Guid>(defaultRole));
            }
            await _userManager.AddToRoleAsync(user, defaultRole);

            // ✅ Cache roles directly
            var cacheKey = _cache.BuildCacheKey("UserRoles", _cacheKeyPrefix, user.Id);
            var roles = new List<string> { defaultRole };
            await _cache.SetAsync(cacheKey, roles, TimeSpan.FromMinutes(30));

            // ✅ Generate tokens
            var context = _httpContextAccessor.HttpContext!;
            var accessToken = _jwtService.GenerateToken(user.Id, user.UserName!, user.Email!, roles);
            var refreshToken = await _refreshTokenService.GenerateTokenAsync(
                user.Id,
                _refreshTokenService.GetIpAdress(context),
                _refreshTokenService.GetDevice(context),
                expiryDays: 15
            );

            _logger.LogInformation("User {UserId} registered successfully with email {Email} from IP {IP}",
                user.Id, user.Email, _refreshTokenService.GetIpAdress(context));

            return new AuthDto
            {
                Message = AppMessages.RegistrationSuccessful,
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
            _logger.LogWarning("Invalid credentials for {Input}", login.EmailORUserName);
            return null!;
        }

        // ✅ Get roles from cache
        var cacheKey = _cache.BuildCacheKey("UserRoles", _cacheKeyPrefix, user.Id);
        var roles = await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            var r = await _userManager.GetRolesAsync(user);
            return r.ToList();
        }, TimeSpan.FromMinutes(30));

        var accessToken = _jwtService.GenerateToken(user.Id, user.UserName!, user.Email!, roles);

        var context = _httpContextAccessor.HttpContext!;
        var device = _refreshTokenService.GetDevice(context);
        var ip = _refreshTokenService.GetIpAdress(context);

        // ✅ نحصل على كل التوكنات لنفس الجهاز (بدون إلغاء النشطة)
        var existingTokens = await _unitOfWork.RepositoryEntity<RefreshToken>()
            .FindAsync(t => t.UserId == user.Id && t.Device == device, true);

        // ❌ حذفنا عملية إلغاء التوكنات القديمة
        // ✅ نكتفي بتنظيف القديم جدًا بعد الحد الأقصى ��لمسموح
        await _refreshTokenService.CleanupOldTokensAsync(user.Id, device, maxTokens: 5);

        // ✅ إنشاء RefreshToken جديد
        var newRefreshToken = await _refreshTokenService.GenerateTokenAsync(
            user.Id, ip, device, expiryDays: 15);

        _logger.LogInformation("User {UserId} logged in successfully from device {Device}", user.Id, device);

        return new RefreshTokenDto
        {
            Message = AppMessages.LoginSuccessful,
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
            _logger.LogWarning("Refresh token is null or empty");
            tokenDTO.Message = _localizer["InvalidToken"];
            return tokenDTO;
        }

        // ✅ البحث عن المستخدم اللي يملك هذا التوكن
        var user = await _userManager.Users
            .Include(u => u.RefreshTokens)
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.RefreshTokens!.Any(rt => rt.RefToken == token));

        if (user is null)
        {
            _logger.LogWarning("No user found for refresh token: {Token}", token);
            tokenDTO.Message = AppMessages.UserNotFound;
            return tokenDTO;
        }

        var refreshToken = user.RefreshTokens!.SingleOrDefault(rt => rt.RefToken == token);

        if (refreshToken == null || !refreshToken.IsActive)
        {
            _logger.LogInformation("Inactive or expired refresh token: {Token}", token);
            tokenDTO.Message = AppMessages.InactiveToken;
            return tokenDTO;
        }

        // ✅ تفعيل الـ Rotation (نوقف القديم ونصدر جديد)
        var revoked = await _refreshTokenService.RevokeTokenAsync(refreshToken.RefToken!);
        if (!revoked)
        {
            _logger.LogWarning("Failed to revoke token: {Token}", token);
            tokenDTO.Message = AppMessages.InvalidRevokedToken;
            return tokenDTO;
        }

        // ✅ تجهيز بيانات المستخدم لتوليد توكن جديد
        var cacheKey = _cache.BuildCacheKey("UserRoles", _cacheKeyPrefix, user.Id);
        var roles = await _cache.GetOrAddAsync(cacheKey, async () =>
        {
            var r = await _userManager.GetRolesAsync(user);
            return r.ToList();
        }, TimeSpan.FromMinutes(30));

        var context = _httpContextAccessor.HttpContext!;
        var newRefreshToken = await _refreshTokenService.GenerateTokenAsync(
            user.Id,
            _refreshTokenService.GetIpAdress(context),
            _refreshTokenService.GetDevice(context),
            expiryDays: 15
        );
         await _refreshTokenService.CleanupOldTokensAsync(user.Id, _refreshTokenService.GetDevice(context), maxTokens: 5);

        var newAccessToken = _jwtService.GenerateToken(user.Id, user.UserName!, user.Email!, roles);

        _logger.LogInformation("Token refreshed successfully for user {UserId}", user.Id);

        tokenDTO.Message = AppMessages.TokenRefreshed;
        tokenDTO.IsActive = true;
        tokenDTO.FullName = user.FullName ?? "Unk";
        tokenDTO.Token = newAccessToken;
        tokenDTO.RefreshToken = newRefreshToken.RefToken ?? string.Empty;
        tokenDTO.ExpiredAt = newRefreshToken.ExpiresAt;

        return tokenDTO;
    }

    // ✅ Logout: Revoke the refresh token  تسجيل الخروج: إلغاء توكن وايقافه 
    public async Task<bool> LogoutAsync(string token, CancellationToken cancellationToken = default)
    {
        try
        {
            var revokedToken = await _refreshTokenService.RevokeTokenAsync(token);
            if (!revokedToken)
            {
                _logger.LogInformation("No refresh token found for logout: {Token}", token);
                return false;
            }

            _logger.LogInformation("User logged out successfully: {Token}", token);
            return revokedToken;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while logging out: {Token}", token);
            return false;
        }
    }

    public async Task<AuthDto> RequestPasswordResetAsync(RequestPasswordResetDto dto, CancellationToken cancellationToken = default)
    {
        // 1️⃣ التحقق من صحة البريد
        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ValidationException("Email is required.");

        // 2️⃣ البحث عن المستخدم بواسطة البريد
        User? user = await _userManager.FindByEmailAsync(dto.Email);

        // 3️⃣ أمان: لو المستخدم غير موجود، نرجع رسالة عامة
        if (user == null)
        {
            return new AuthDto
            {
                Message = "If this email is registered, you will receive a password reset link."
            };
        }

        // 4️⃣ توليد رمز إعادة تعيين كلمة المرور
        var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

        // 5️⃣ بناء رابط إعادة التعيين
        var apiBaseUrl = _configuration["AppSettings:ApiBaseUrl"];
        if (string.IsNullOrWhiteSpace(apiBaseUrl))
            throw new InvalidOperationException("API Base URL is not configured.");

        var encodedToken = Uri.EscapeDataString(resetToken);
        var encodedEmail = Uri.EscapeDataString(user.Email!); // تأكد أن البريد ليس null

        var resetLink = $"{apiBaseUrl}/EmailTemplates/ResetPasswordPage.html?" +
               $"?email={Uri.EscapeDataString(user.Email!)}" +
               $"&token={Uri.EscapeDataString(resetToken)}" +
               $"&username={Uri.EscapeDataString(user.UserName!)}" +
               $"&profileImage={Uri.EscapeDataString(user.ProfileImageUrl!)}";

        // 6️⃣ إرسال البريد باستخدام EmailService
        await _emailService.SendPasswordResetAsync(user.Gmail!, resetLink);

        // 7️⃣ إرجاع رسالة عامة لتجنب كشف وجود البريد
        return new AuthDto
        {
            Message = "If this email is registered, you will receive a password reset link.",
            IsAuthenticated = true,
            FullName = user.FullName ?? "UnKwon",
            Email = user.Email ?? "UnKwon",
            UserName = user.UserName ?? "Unkwon",
            ProfileImageURL = user.ProfileImageUrl ?? string.Empty,
        };
    }

    public async Task<AuthDto> ResetPasswordAsync(ResetPasswordDto dto, CancellationToken cancellationToken = default)
    {
        if (dto.NewPassword != dto.ConfirmNewPassword)
            throw new ValidationException("New Password and Confirm Password do not match.");

        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
            throw new KeyNotFoundException("Invalid email.");

        var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.NewPassword);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Password reset failed: {errors}");
        }

        return new AuthDto
        {
            Message = "Password reset successfully.",
            IsAuthenticated = true,
            FullName = user.FullName ?? "Unknown",
            UserName = user.UserName ?? "Unknown",
            Email = user.Email ?? "Unknown",
            ProfileImageURL = user.ProfileImageUrl ?? string.Empty
        };
    }

    public async Task<AuthDto> ChangePasswordAsync(Guid userId, ChangePasswordDto dto, CancellationToken cancellationToken = default)
    {
        if(dto.NewPassword != dto.ConfirmNewPassword)
            throw new ValidationException("New Password and Confirm New Password do not match.");

        if(dto.NewPassword == dto.CurrentPassword)
            throw new ValidationException("New Password must be different from Current Password.");

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            throw new KeyNotFoundException("User not found.");

        var isCurrentPasswordValid = await _userManager.CheckPasswordAsync(user, dto.CurrentPassword!);
        if (!isCurrentPasswordValid)
            throw new ValidationException("Current Password is incorrect.");

        var result = await _userManager.ChangePasswordAsync(user, dto.CurrentPassword!, dto.NewPassword!);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Password change failed: {errors}");
        }
        return new AuthDto
        {
            Message = AppMessages.PasswordChangeSuccessful,
            IsAuthenticated = true,
            FullName = user.FullName ?? "Unk",
            UserName = user.UserName ?? "Unk",
            Email = user.Email ?? "Unk",
            ProfileImageURL = user.ProfileImageUrl ?? string.Empty
        };
    }

    public Task<AuthDto> SendEmailVerificationAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<AuthDto> VerifyEmailAsync(VerifyEmailDto dto, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

}

