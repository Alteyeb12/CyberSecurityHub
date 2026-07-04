using CyberInsight.Models;
using CyberInsight.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CyberInsight.Controllers
{
    [Authorize(AuthenticationSchemes = "CyberInsightScheme")]
    public class DashboardController : Controller
    {
        private readonly IThreatMonitoringService _threatService;
        private readonly ILogger<DashboardController> _logger;

        public DashboardController(IThreatMonitoringService threatService, ILogger<DashboardController> logger)
        {
            _threatService = threatService;
            _logger = logger;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();
            var threats = await _threatService.GetUserThreatsAsync(userId);
            var criticalThreats = await _threatService.GetCriticalThreatsAsync(userId);
            var statistics = await _threatService.GetThreatStatisticsAsync(userId);

            var viewModel = new DashboardViewModel
            {
                TotalThreats = threats.Count,
                CriticalThreats = criticalThreats.Count,
                RecentThreats = threats.Take(10).ToList(),
                ThreatStatistics = statistics
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Threats()
        {
            var userId = GetUserId();
            var threats = await _threatService.GetUserThreatsAsync(userId);
            return View(threats);
        }

        [HttpPost]
        public async Task<IActionResult> ResolveThreat(int alertId, string notes)
        {
            var result = await _threatService.ResolveThreatAsync(alertId, notes);
            if (!result)
                return BadRequest();

            return Ok();
        }
    }

    public class DashboardViewModel
    {
        public int TotalThreats { get; set; }
        public int CriticalThreats { get; set; }
        public List<ThreatAlert> RecentThreats { get; set; } = new();
        public Dictionary<string, int> ThreatStatistics { get; set; } = new();
    }
}