using System.Security.Claims;

namespace MarketPOS.Application.Services.InterfacesServices.InterFacesAuthentication;
public interface IJwtService
{
    string GenerateToken(Guid userId, string userName, string email, IList<string> roles);
    ClaimsPrincipal? ValidateToken(string token);
}
