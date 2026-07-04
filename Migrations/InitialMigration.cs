using CyberInsight.Data;
using Microsoft.EntityFrameworkCore;

namespace CyberInsight.Migrations
{
    public class InitialMigration
    {
        public static void Initialize(ApplicationDbContext context)
        {
            try
            {
                // Apply all pending migrations
                context.Database.Migrate();
                Console.WriteLine("✅ Database migrations applied successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Migration error: {ex.Message}");
                throw;
            }
        }
    }
}
