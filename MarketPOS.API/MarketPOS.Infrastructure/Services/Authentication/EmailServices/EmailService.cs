using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MarketPOS.Infrastructure.Services.Authentication.EmailServices;
// Using configuration for  settings
public class EmailService : IEmailService
{
    private readonly EmailSettings _settings;
    private readonly string _webRootPath;

    public EmailService(IOptions<EmailSettings> settings,  string webRootPath)
    { 
        _settings = settings.Value;
        _webRootPath = webRootPath;
    }

    private async Task SendEmailInternalAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SmtpUser));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new TextPart("html") { Text = body };

        using var client = new SmtpClient();
        await client.ConnectAsync
            (_settings.SmtpServer, _settings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
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
        var body = $"<p>Click the link below to verify your email:</p>"+
            $"<p><a href='{verificationLink}'>Verify Email</a></p>";
        await SendEmailInternalAsync(email, subject, body);
    }

    public async Task SendPasswordResetAsync(string email, string resetLink)
    {
        var subject = "Reset your password";

        // تحميل القالب واستبدال المتغير
        var bodyTemplate = LoadTemplate("PasswordReset.html");
        var body = bodyTemplate.Replace("{{ResetLink}}", resetLink);

        await SendEmailInternalAsync(email, subject, body);
    }


    private string LoadTemplate(string templateName)
    {
        var path = Path.Combine(_webRootPath, "EmailTemplates", templateName);

        if (!File.Exists(path))
            throw new FileNotFoundException($"Template file not found: {path}");

        return File.ReadAllText(path);
    }

}