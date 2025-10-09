using MarketPOS.Application.Services.InterfacesServices.Authentication;
using MarketPOS.Infrastructure.Services.Authentication.EmailServices;
using Microsoft.Extensions.Options;

namespace MarketPOS.API.Extensions.SetteingEmailService
{
    public static class SetteingEmail
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {

          services.Configure<EmailSettings>(configuration.GetSection("EmailSettings"));

            // Get the web root path
            var webRootPath = env.WebRootPath;

            // Register EmailService with the web root path
            services.AddTransient<IEmailService>(provider =>
            {
                var options = provider.GetRequiredService<IOptions<EmailSettings>>();
                return new EmailService(options, webRootPath);
            });

            return services;
        }
    }
}
