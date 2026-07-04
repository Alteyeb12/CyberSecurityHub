using CyberInsight.Data;
using CyberInsight.Helpers;

namespace CyberInsight.Extensions
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddCustomServices(this IServiceCollection services)
        {
            // الخدمات المخصصة تم تسجيلها بالفعل في Program.cs
            return services;
        }

        public static WebApplication ApplyMigrations(this WebApplication app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<ApplicationDbContext>();

                try
                {
                    // تطبيق التهجيرات
                    context.Database.Migrate();

                    // إضافة بيانات تجريبية
                    DataSeedingHelper.SeedData(context);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger>();
                    logger.LogError(ex, "An error occurred during migration");
                }
            }

            return app;
        }
    }
}
