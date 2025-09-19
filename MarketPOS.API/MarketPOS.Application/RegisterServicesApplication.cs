public static class RegisterServicesApplication
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assemblies = AppDomain.CurrentDomain
               .GetAssemblies()
               .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
               .ToArray();

        // ✅ تسجيل AutoMapper (لـ DTO ↔️ Entity)
        services.AddAutoMapper(cfg => { }, assemblies);

        // ✅ تسجيل MediatR ومعالجة كل الـ Handlers العادية
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(assemblies));

        // ✅ تسجيل جميع Validators باستخدام FluentValidation
        services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
        // ✅ تسجيل الـ Pipeline Behavior لتفعيل التحقق التلقائي من الـ Validators
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}

