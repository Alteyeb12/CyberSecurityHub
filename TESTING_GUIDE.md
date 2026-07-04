# متطلبات الاختبار

## اختبار الوحدة (Unit Tests)

```csharp
[TestClass]
public class AuthServiceTests
{
    private AuthService _authService;
    private ApplicationDbContext _context;

    [TestInitialize]
    public void Setup()
    {
        // إعداد قاعدة بيانات مؤقتة للاختبار
    }

    [TestMethod]
    public async Task RegisterAsync_WithValidData_ReturnsTrue()
    {
        // Arrange
        var email = "test@example.com";
        var password = "Password123";

        // Act
        var result = await _authService.RegisterAsync(
            email, "Test", "User", password, "TestCo"
        );

        // Assert
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task AuthenticateAsync_WithInvalidPassword_ReturnsNull()
    {
        // Arrange
        var email = "test@example.com";
        var password = "WrongPassword";

        // Act
        var result = await _authService.AuthenticateAsync(email, password);

        // Assert
        Assert.IsNull(result);
    }
}
```

## اختبار التكامل (Integration Tests)

```csharp
[TestClass]
public class ThreatMonitoringTests
{
    private IThreatMonitoringService _threatService;

    [TestMethod]
    public async Task CreateThreatAlert_SavesToDatabase()
    {
        // Arrange
        int userId = 1;
        var threatType = "SQL Injection";

        // Act
        var threat = await _threatService.CreateThreatAlertAsync(
            userId, threatType, "Test description",
            ThreatSeverity.High, "192.168.1.1", "Web Server"
        );

        // Assert
        Assert.IsNotNull(threat);
        Assert.AreEqual(threatType, threat.ThreatType);
    }
}
```

## اختبار الأمان (Security Tests)

- ✓ اختبار تشفير كلمات المرور
- ✓ اختبار SQL Injection
- ✓ اختبار Cross-Site Scripting (XSS)
- ✓ اختبار Cross-Site Request Forgery (CSRF)
- ✓ اختبار المصادقة والتصريح

## اختبار الأداء (Performance Tests)

- تحميل قاعدة البيانات بـ 10,000 تهديد
- قياس وقت الاستجابة
- اختبار التنافس (Concurrency)
- اختبار استهلاك الذاكرة
