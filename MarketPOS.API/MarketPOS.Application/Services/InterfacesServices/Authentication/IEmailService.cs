namespace MarketPOS.Application.Services.InterfacesServices.Authentication;

public interface IEmailService
{
    Task SendPasswordResetAsync(string email, string resetLink);
    Task SendEmailVerificationAsync(string email, string verificationLink);
    Task SendCustomEmailAsync(string email, string subject, string body);
}

