using CyberInsight.Models;
using CyberInsight.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CyberInsight.Tests
{
    [TestClass]
    public class AuthServiceTests
    {
        private Mock<IAuthService> _mockAuthService;

        [TestInitialize]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthService>();
        }

        [TestMethod]
        public async Task RegisterAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var email = "test@example.com";
            var firstName = "Test";
            var lastName = "User";
            var password = "Password123";
            var companyName = "TestCompany";

            _mockAuthService.Setup(s => s.RegisterAsync(
                email, firstName, lastName, password, companyName))
                .ReturnsAsync(true);

            // Act
            var result = await _mockAuthService.Object.RegisterAsync(
                email, firstName, lastName, password, companyName);

            // Assert
            Assert.IsTrue(result);
            _mockAuthService.Verify(s => s.RegisterAsync(
                email, firstName, lastName, password, companyName), Times.Once);
        }

        [TestMethod]
        public async Task RegisterAsync_WithDuplicateEmail_ReturnsFalse()
        {
            // Arrange
            var email = "existing@example.com";
            var firstName = "Test";
            var lastName = "User";
            var password = "Password123";
            var companyName = "TestCompany";

            _mockAuthService.Setup(s => s.RegisterAsync(
                email, firstName, lastName, password, companyName))
                .ReturnsAsync(false);

            // Act
            var result = await _mockAuthService.Object.RegisterAsync(
                email, firstName, lastName, password, companyName);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public async Task AuthenticateAsync_WithValidCredentials_ReturnsUser()
        {
            // Arrange
            var email = "test@example.com";
            var password = "Password123";
            var user = new User 
            { 
                Id = 1, 
                Email = email, 
                FirstName = "Test",
                LastName = "User",
                PasswordHash = "hashedpassword"
            };

            _mockAuthService.Setup(s => s.AuthenticateAsync(email, password))
                .ReturnsAsync(user);

            // Act
            var result = await _mockAuthService.Object.AuthenticateAsync(email, password);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(email, result.Email);
        }

        [TestMethod]
        public async Task AuthenticateAsync_WithInvalidPassword_ReturnsNull()
        {
            // Arrange
            var email = "test@example.com";
            var password = "WrongPassword";

            _mockAuthService.Setup(s => s.AuthenticateAsync(email, password))
                .ReturnsAsync((User)null);

            // Act
            var result = await _mockAuthService.Object.AuthenticateAsync(email, password);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void HashPassword_CreatesSecureHash()
        {
            // Arrange
            var password = "MySecurePassword123";
            _mockAuthService.Setup(s => s.HashPassword(password))
                .Returns("$2a$12$hashedvalue");

            // Act
            var hash = _mockAuthService.Object.HashPassword(password);

            // Assert
            Assert.IsNotNull(hash);
            Assert.AreNotEqual(password, hash);
        }

        [TestMethod]
        public void VerifyPassword_WithCorrectPassword_ReturnsTrue()
        {
            // Arrange
            var password = "MyPassword123";
            var hash = "$2a$12$hashedvalue";

            _mockAuthService.Setup(s => s.VerifyPassword(password, hash))
                .Returns(true);

            // Act
            var result = _mockAuthService.Object.VerifyPassword(password, hash);

            // Assert
            Assert.IsTrue(result);
        }
    }

    [TestClass]
    public class ThreatMonitoringServiceTests
    {
        private Mock<IThreatMonitoringService> _mockThreatService;

        [TestInitialize]
        public void Setup()
        {
            _mockThreatService = new Mock<IThreatMonitoringService>();
        }

        [TestMethod]
        public async Task CreateThreatAlertAsync_WithValidData_ReturnsAlert()
        {
            // Arrange
            var userId = 1;
            var threatType = "SQL Injection";
            var description = "Attempted SQL injection on login form";
            var severity = ThreatSeverity.High;
            var sourceIP = "192.168.1.100";
            var targetAsset = "Web Server";

            var expectedAlert = new ThreatAlert
            {
                Id = 1,
                UserId = userId,
                ThreatType = threatType,
                Description = description,
                Severity = severity,
                SourceIP = sourceIP,
                TargetAsset = targetAsset
            };

            _mockThreatService.Setup(s => s.CreateThreatAlertAsync(
                userId, threatType, description, severity, sourceIP, targetAsset))
                .ReturnsAsync(expectedAlert);

            // Act
            var result = await _mockThreatService.Object.CreateThreatAlertAsync(
                userId, threatType, description, severity, sourceIP, targetAsset);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(threatType, result.ThreatType);
            Assert.AreEqual(severity, result.Severity);
        }

        [TestMethod]
        public async Task GetUserThreatsAsync_ReturnsListOfThreats()
        {
            // Arrange
            var userId = 1;
            var threats = new List<ThreatAlert>
            {
                new ThreatAlert { Id = 1, ThreatType = "SQL Injection", UserId = userId },
                new ThreatAlert { Id = 2, ThreatType = "Brute Force", UserId = userId }
            };

            _mockThreatService.Setup(s => s.GetUserThreatsAsync(userId))
                .ReturnsAsync(threats);

            // Act
            var result = await _mockThreatService.Object.GetUserThreatsAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetCriticalThreatsAsync_ReturnsCriticalThreatsOnly()
        {
            // Arrange
            var userId = 1;
            var criticalThreats = new List<ThreatAlert>
            {
                new ThreatAlert { Id = 1, Severity = ThreatSeverity.Critical, UserId = userId }
            };

            _mockThreatService.Setup(s => s.GetCriticalThreatsAsync(userId))
                .ReturnsAsync(criticalThreats);

            // Act
            var result = await _mockThreatService.Object.GetCriticalThreatsAsync(userId);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.All(t => t.Severity == ThreatSeverity.Critical));
        }

        [TestMethod]
        public async Task ResolveThreatAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var alertId = 1;
            var notes = "Threat resolved";

            _mockThreatService.Setup(s => s.ResolveThreatAsync(alertId, notes))
                .ReturnsAsync(true);

            // Act
            var result = await _mockThreatService.Object.ResolveThreatAsync(alertId, notes);

            // Assert
            Assert.IsTrue(result);
        }
    }

    [TestClass]
    public class StripeServiceTests
    {
        private Mock<IStripeService> _mockStripeService;

        [TestInitialize]
        public void Setup()
        {
            _mockStripeService = new Mock<IStripeService>();
        }

        [TestMethod]
        public async Task CreateCustomerAsync_WithValidData_ReturnsCustomerId()
        {
            // Arrange
            var email = "customer@example.com";
            var name = "John Doe";
            var expectedCustomerId = "cus_123456";

            _mockStripeService.Setup(s => s.CreateCustomerAsync(email, name))
                .ReturnsAsync(expectedCustomerId);

            // Act
            var result = await _mockStripeService.Object.CreateCustomerAsync(email, name);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCustomerId, result);
        }

        [TestMethod]
        public async Task CreateSubscriptionAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            var plan = "Professional";
            var amount = 99.99m;

            _mockStripeService.Setup(s => s.CreateSubscriptionAsync(userId, plan, amount))
                .ReturnsAsync(true);

            // Act
            var result = await _mockStripeService.Object.CreateSubscriptionAsync(userId, plan, amount);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public async Task CancelSubscriptionAsync_WithValidData_ReturnsTrue()
        {
            // Arrange
            var userId = 1;
            _mockStripeService.Setup(s => s.CancelSubscriptionAsync(userId))
                .ReturnsAsync(true);

            // Act
            var result = await _mockStripeService.Object.CancelSubscriptionAsync(userId);

            // Assert
            Assert.IsTrue(result);
        }
    }
}
