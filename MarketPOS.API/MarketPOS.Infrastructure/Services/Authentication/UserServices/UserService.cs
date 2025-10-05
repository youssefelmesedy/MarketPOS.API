using Market.Domain.Entities.Auth;
using MarketPOS.Application.InterfaceCacheing;
using MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
using Microsoft.AspNetCore.Identity;

namespace MarketPOS.Infrastructure.Services.Authentication.UserServices;
public class UserService :  IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IGenericCache _cache;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UserService> _logger;
    private readonly string _prefixCacheKey;
    public UserService(
        IUnitOfWork unitOfWork,
        ILogger<UserService> logger,
        IGenericCache cache = null!,
        UserManager<User> userManager = null!) 
    {
        _unitOfWork= unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _cache = cache?? throw new ArgumentNullException(nameof(cache));
        _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _prefixCacheKey = typeof(UserService).Name;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        try
        {
            var cachekey = _cache.BuildCacheKey(_prefixCacheKey, email);

            return await _cache.GetOrAddAsync(cachekey, async () =>
            {
                _logger.LogInformation("Get user By Email Sucssesfully");
                return await _userManager.FindByEmailAsync(email);
            }, TimeSpan.FromMinutes(10)); // Cache for 10 minute

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching user by email");
            throw;
        }
    }

    public Task<User> GetUserAsync(Guid id)
    {
        var cachekey = _cache.BuildCacheKey(_prefixCacheKey, id);

        return _cache.GetOrAddAsync(cachekey, async () =>
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                throw new KeyNotFoundException($"User with ID {id} not found.");
            }
            return user;
        }, TimeSpan.FromMinutes(10)); // Cache for 10 minute
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        try
        {
            var cachekey = _cache.BuildCacheKey(_prefixCacheKey, username);
            return await _cache.GetOrAddAsync(cachekey, async () =>
            {
                return await _userManager.FindByNameAsync(username);
            }, TimeSpan.FromMinutes(10)); // Cache for 10 minute
        }
        catch (Exception ex)
        {
            // Log the exception
            _logger.LogError(ex, "Error occurred while fetching user by username");
            throw;
        }
    }

    public Task AddUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task UpdateUserAsync(User user)
    {
        throw new NotImplementedException();
    }

    public Task DeleteUserAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
