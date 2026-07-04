using CyberInsight.Data;
using CyberInsight.Models;

namespace CyberInsight.Helpers
{
    public class DataSeedingHelper
    {
        public static void SeedData(ApplicationDbContext context)
        {
            // فقط إذا كانت قاعدة البيانات فارغة
            if (context.Users.Any())
                return;

            // إضافة مستخدم تجريبي
            var testUser = new User
            {
                Email = "test@cyberinsight.com",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = "$2a$12$R9h7cIPz0gi.URNNX3kh2OPST9EgwvJ9BtD5.3nR2pz0T6fJmVhH2", // password: Test@123
                CompanyName = "Test Company",
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };
            context.Users.Add(testUser);
            context.SaveChanges();

            // إضافة اشتراك للمستخدم التجريبي
            var subscription = new Subscription
            {
                UserId = testUser.Id,
                Plan = SubscriptionPlan.Professional,
                Status = SubscriptionStatus.Active,
                Price = 99.99m,
                StartDate = DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow
            };
            context.Subscriptions.Add(subscription);

            // إضافة تهديدات تجريبية
            var threats = new List<ThreatAlert>
            {
                new ThreatAlert
                {
                    UserId = testUser.Id,
                    ThreatType = "SQL Injection",
                    Description = "محاولة حقن SQL على نموذج تسجيل الدخول",
                    Severity = ThreatSeverity.High,
                    SourceIP = "203.0.113.45",
                    TargetAsset = "Web Server",
                    DetectedAt = DateTime.UtcNow.AddHours(-2),
                    AlertCount = 3
                },
                new ThreatAlert
                {
                    UserId = testUser.Id,
                    ThreatType = "Brute Force",
                    Description = "محاولات تسجيل دخول متعددة فاشلة",
                    Severity = ThreatSeverity.Medium,
                    SourceIP = "192.168.1.100",
                    TargetAsset = "Authentication Service",
                    DetectedAt = DateTime.UtcNow.AddHours(-1),
                    AlertCount = 5
                },
                new ThreatAlert
                {
                    UserId = testUser.Id,
                    ThreatType = "DDoS Attack",
                    Description = "هجوم حجب الخدمة الموزع",
                    Severity = ThreatSeverity.Critical,
                    SourceIP = "198.51.100.50",
                    TargetAsset = "Load Balancer",
                    DetectedAt = DateTime.UtcNow.AddMinutes(-30),
                    AlertCount = 1
                }
            };
            context.ThreatAlerts.AddRange(threats);

            context.SaveChanges();
        }
    }
}
