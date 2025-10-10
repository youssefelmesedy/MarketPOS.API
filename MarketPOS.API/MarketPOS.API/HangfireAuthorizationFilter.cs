using Hangfire.Dashboard;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true; // ⛔ مؤقتًا مسموح للجميع (يفضل لاحقًا ربطها بـ Role)
    }
}
