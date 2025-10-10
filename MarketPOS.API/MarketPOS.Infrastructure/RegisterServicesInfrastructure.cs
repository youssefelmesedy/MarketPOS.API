using Hangfire;
using MarketPOS.Infrastructure.Context.Persistence;
using MarketPOS.Infrastructure.Services.Authentication.AuthenticationService;
using MarketPOS.Infrastructure.Services.Authentication.JWTServices;
using MarketPOS.Infrastructure.Services.FileStorage;
using Microsoft.Extensions.Hosting;

namespace MarketPOS.Infrastructure
{
    public static class RegisterServicesInfrastructure
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment)
        {
            // 1️⃣ إعداد قاعدة البيانات
            services.AddDatabaseContext(configuration, environment);

            // 2️⃣ إعداد Hangfire
            services.AddHangfireConfiguration(configuration, environment);

            // 3️⃣ إعداد Identity
            services.AddIdentityConfiguration();

            // 4️⃣ إضافة الـ Services والـ Repositories
            services.AddApplicationDependencies();

            Console.WriteLine($"✅ Environment: {environment.EnvironmentName}");

            return services;
        }

        #region Database Configuration
        private static IServiceCollection AddDatabaseContext(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment)
        {
            var connectionString = environment.EnvironmentName == "Development"
                ? configuration.GetConnectionString("LocalConnection")
                : configuration.GetConnectionString("ServerConnection");

            Console.WriteLine($"🔗 Connection: {connectionString}");

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    sqlOptions.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            });

            return services;
        }
        #endregion

        #region Hangfire Configuration
        private static IServiceCollection AddHangfireConfiguration(
            this IServiceCollection services,
            IConfiguration configuration,
            IHostEnvironment environment)
        {
            var connectionString = environment.EnvironmentName == "Development"
                ? configuration.GetConnectionString("LocalConnection")
                : configuration.GetConnectionString("ServerConnection");

            services.AddHangfire(config =>
            {
                config.SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                      .UseSimpleAssemblyNameTypeSerializer()
                      .UseRecommendedSerializerSettings()
                      .UseSqlServerStorage(connectionString, new Hangfire.SqlServer.SqlServerStorageOptions
                      {
                          CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                          SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                          QueuePollInterval = TimeSpan.Zero,
                          UseRecommendedIsolationLevel = true,
                          DisableGlobalLocks = true
                      });
            });

            // ✅ تشغيل السيرفر
            services.AddHangfireServer(options =>
            {
                options.WorkerCount = Environment.ProcessorCount * 2; // عدد الـ Workers
                options.ServerName = $"HangfireServer-{environment.EnvironmentName}";
                options.Queues = new[] { "default", "emails", "notifications", "background" };
            });

            return services;
        }
        #endregion

        #region Identity Configuration
        private static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole<Guid>>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredLength = 6;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

            return services;
        }
        #endregion

        #region Dependency Injection Configuration
        private static IServiceCollection AddApplicationDependencies(this IServiceCollection services)
        {
            // JWT & Auth
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IFileService, FileService>();
            services.AddScoped<IAuthService, AuthService>();

            // UnitOfWork
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Generic Repositories
            services.AddScoped(typeof(IFullRepository<>), typeof(GenericRepository<>));
            services.AddScoped(typeof(IFullService<>), typeof(GenericService<>));
            services.AddScoped(typeof(IFullService<>), typeof(GenericServiceCacheing<>));

            // Repositories
            services.AddScoped<IProductRepo, ProductRepository>();
            services.AddScoped<IProductPriceRepo, ProductPriceRepository>();
            services.AddScoped<IProductUnitProfileRepo, ProductUnitProfileRepository>();
            services.AddScoped<ICategoryRepo, CategoryRepository>();
            services.AddScoped<IActivelngredinentsRepo, ActiveingredinentRepository>();
            services.AddScoped<IWareHouseRepo, WareHouseRepository>();
            services.AddScoped<ISupplierRepo, SupplierRepo>();

            // Services
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductPriceService, ProductPriceService>();
            services.AddScoped<IProductUnitProfileService, ProductUnitProfileService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IActiveingredinentService, ActiveingredinentService>();
            services.AddScoped<IWareHouseService, WareHouseService>();
            services.AddScoped<ISupplierService, SupplierService>();
            services.AddScoped<IAggregateService, AggregateService>();

            return services;
        }
        #endregion
    }
}
