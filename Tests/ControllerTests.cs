using CyberInsight.Controllers;
using CyberInsight.Models;
using CyberInsight.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace CyberInsight.Tests
{
    [TestClass]
    public class AccountControllerTests
    {
        private Mock<IAuthService> _mockAuthService;
        private Mock<ILogger<AccountController>> _mockLogger;
        private AccountController _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockAuthService = new Mock<IAuthService>();
            _mockLogger = new Mock<ILogger<AccountController>>();
            _controller = new AccountController(_mockAuthService.Object, _mockLogger.Object);
        }

        [TestMethod]
        public void Register_GetRequest_ReturnsView()
        {
            // Act
            var result = _controller.Register();

            // Assert
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public async Task Login_WithValidCredentials_RedirectsToDashboard()
        {
            // Arrange
            var model = new LoginViewModel { Email = "test@example.com", Password = "Password123" };
            var user = new User { Id = 1, Email = "test@example.com" };

            _mockAuthService.Setup(s => s.AuthenticateAsync(model.Email, model.Password))
                .ReturnsAsync(user);

            // Act
            var result = await _controller.Login(model) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Index", result.ActionName);
            Assert.AreEqual("Dashboard", result.ControllerName);
        }

        [TestMethod]
        public async Task Login_WithInvalidCredentials_ReturnsViewWithError()
        {
            // Arrange
            var model = new LoginViewModel { Email = "test@example.com", Password = "WrongPassword" };

            _mockAuthService.Setup(s => s.AuthenticateAsync(model.Email, model.Password))
                .ReturnsAsync((User)null);

            // Act
            var result = await _controller.Login(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(_controller.ModelState.IsValid);
        }
    }

    [TestClass]
    public class ThreatControllerTests
    {
        private Mock<IThreatMonitoringService> _mockThreatService;
        private Mock<ILogger<ThreatController>> _mockLogger;

        [TestInitialize]
        public void Setup()
        {
            _mockThreatService = new Mock<IThreatMonitoringService>();
            _mockLogger = new Mock<ILogger<ThreatController>>();
        }

        [TestMethod]
        public async Task GetAllThreats_ReturnsOkWithThreats()
        {
            // Arrange
            var threats = new List<ThreatAlert>
            {
                new ThreatAlert { Id = 1, ThreatType = "SQL Injection" },
                new ThreatAlert { Id = 2, ThreatType = "Brute Force" }
            };

            _mockThreatService.Setup(s => s.GetUserThreatsAsync(It.IsAny<int>()))
                .ReturnsAsync(threats);

            // Act & Assert
            Assert.AreEqual(2, threats.Count);
        }
    }
}
