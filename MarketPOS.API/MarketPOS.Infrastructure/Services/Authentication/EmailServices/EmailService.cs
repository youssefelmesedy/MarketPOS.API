using MailKit.Net.Smtp;
using MarketPOS.Application.Services.InterfacesServices.Authentication;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using static Org.BouncyCastle.Crypto.Engines.SM2Engine;

namespace MarketPOS.Infrastructure.Services.Authentication.EmailServices;

//HardCoded SMTP settings for simplicity. In production, use secure configuration management.
#region HardCoded SMTP settings for simplicityusing System;
//public class EmailService : IEmailService
//{
//    // ⚡ ضبط إعدادات SMTP هنا (مثلاً Gmail)
//    private readonly string _smtpServer = "smtp.gmail.com";
//    private readonly int _smtpPort = 587;
//    private readonly string _smtpUser = "your-email@gmail.com"; // البريد اللي هيبعت الإيميلات
//    private readonly string _smtpPass = "your-app-password";    // App Password لو Gmail

//    private async Task SendEmailInternalAsync(string to, string subject, string body)
//    {
//        var message = new MimeMessage();
//        message.From.Add(new MailboxAddress("YourApp", _smtpUser));
//        message.To.Add(MailboxAddress.Parse(to));
//        message.Subject = subject;

//        message.Body = new TextPart("html")
//        {
//            Text = body
//        };

//        using var client = new SmtpClient();
//        await client.ConnectAsync(_smtpServer, _smtpPort, MailKit.Security.SecureSocketOptions.StartTls);
//        await client.AuthenticateAsync(_smtpUser, _smtpPass);
//        await client.SendAsync(message);
//        await client.DisconnectAsync(true);
//    }

//    public async Task SendCustomEmailAsync(string email, string subject, string body)
//    {
//        await SendEmailInternalAsync(email, subject, body);
//    }

//    public async Task SendEmailVerificationAsync(string email, string verificationLink)
//    {
//        var subject = "Verify your email address";
//        var body = $"<p>Click the link below to verify your email:</p><p><a href='{verificationLink}'>Verify Email</a></p>";
//        await SendEmailInternalAsync(email, subject, body);
//    }

//    public async Task SendPasswordResetAsync(string email, string resetLink)
//    {
//        var subject = "Reset your password";
//        var body = $"<p>Click the link below to reset your password:</p><p><a href='{resetLink}'>Reset Password</a></p>";
//        await SendEmailInternalAsync(email, subject, body);
//    }
//}
#endregion

// Using configuration for  settings
public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;

    public EmailService(IOptions<EmailSettings> settings)
    {
        _settings = settings.Value;
    }

    private async Task SendEmailInternalAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SmtpUser));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync(_settings.SmtpServer, _settings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
        await client.AuthenticateAsync(_settings.SmtpUser, _settings.SmtpPass);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

    public async Task SendCustomEmailAsync(string email, string subject, string body)
    {
        await SendEmailInternalAsync(email, subject, body);
    }

    public async Task SendEmailVerificationAsync(string email, string verificationLink)
    {
        var subject = "Verify your email address";
        var body = $"<p>Click the link below to verify your email:</p><p><a href='{verificationLink}'>Verify Email</a></p>";
        await SendEmailInternalAsync(email, subject, body);
    }

    public async Task SendPasswordResetAsync(string email, string resetLink)
    {
        var subject = "Reset your password";
        var body = $"<p>Click the link below to reset your password:</p><p><a href='{resetLink}'>Reset Password</a></p>";
        await SendEmailInternalAsync(email, subject, body);
    }
}