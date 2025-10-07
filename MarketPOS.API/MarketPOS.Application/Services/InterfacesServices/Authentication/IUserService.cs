using Market.Domain.Entities.Auth;

namespace MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
public interface IUserService 
{
    Task<User> GetUserAsync(Guid id);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByEmailAsync(string email);
    Task AddUserAsync(User user);
    Task UpdateUserAsync(User user);
    Task DeleteUserAsync(Guid id);
}
