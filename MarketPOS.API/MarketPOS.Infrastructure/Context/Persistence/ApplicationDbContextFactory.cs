using MarketPOS.Infrastructure.Context.Persistence;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace MarketPOS.Infrastructure.Persistence
{
    public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            // ✅ 1. تحديد مجلد الـ API الرئيسي (عشان يقرأ appsettings من المشروع الأساسي)
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "../MarketPOS.API");

            // ✅ 2. محاولة قراءة البيئة من عدة أماكن بشكل ديناميكي
            var environment =
                Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? // 1️⃣ لو متغير البيئة موجود
                (File.Exists(Path.Combine(basePath, "launchSettings.json"))
                    ? ExtractEnvironmentFromLaunchSettings(basePath)              // 2️⃣ لو موجود في launchSettings.json
                    : "Production");                                             // 3️⃣ افتراضيًا Production

            // ✅ 3. تحميل ملفات الإعدادات المناسبة
            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddJsonFile("appsettings.JWT.json", optional: true)
                .AddJsonFile("appsettings.Email.json", optional: true)
                .AddEnvironmentVariables()
                .Build();

            // ✅ 4. اختيار connection string المناسب
            var connectionString = environment == "Development"
                ? config.GetConnectionString("LocalConnection")
                : config.GetConnectionString("ServerConnection");

            Console.WriteLine($"🌍 Environment: {environment}");
            Console.WriteLine($"🧱 Using connection: {connectionString}");

            // ✅ 5. تجهيز DbContext
            var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new ApplicationDbContext(optionsBuilder.Options);
        }

        // 🔍 دالة مساعدة لقراءة البيئة من launchSettings.json (لو متغير البيئة مش متعرف)
        private static string ExtractEnvironmentFromLaunchSettings(string basePath)
        {
            var launchSettingsPath = Path.Combine(basePath, "Properties", "launchSettings.json");
            if (!File.Exists(launchSettingsPath))
                return "Production";

            var json = File.ReadAllText(launchSettingsPath);
            if (json.Contains("Development", StringComparison.OrdinalIgnoreCase))
                return "Development";
            if (json.Contains("Staging", StringComparison.OrdinalIgnoreCase))
                return "Staging";

            return "Production";
        }
    }
}
