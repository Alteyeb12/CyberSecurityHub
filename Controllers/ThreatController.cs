using CyberInsight.Models;
using CyberInsight.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CyberInsight.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "CyberInsightScheme")]
    public class ThreatController : ControllerBase
    {
        private readonly IThreatMonitoringService _threatService;
        private readonly ILogger<ThreatController> _logger;

        public ThreatController(IThreatMonitoringService threatService, ILogger<ThreatController> logger)
        {
            _threatService = threatService;
            _logger = logger;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<ThreatAlert>>> GetAllThreats()
        {
            var userId = GetUserId();
            var threats = await _threatService.GetUserThreatsAsync(userId);
            return Ok(threats);
        }

        [HttpGet("critical")]
        public async Task<ActionResult<List<ThreatAlert>>> GetCriticalThreats()
        {
            var userId = GetUserId();
            var threats = await _threatService.GetCriticalThreatsAsync(userId);
            return Ok(threats);
        }

        [HttpGet("statistics")]
        public async Task<ActionResult<Dictionary<string, int>>> GetStatistics()
        {
            var userId = GetUserId();
            var stats = await _threatService.GetThreatStatisticsAsync(userId);
            return Ok(stats);
        }

        [HttpPost("create")]
        public async Task<ActionResult<ThreatAlert>> CreateThreat([FromBody] CreateThreatRequest request)
        {
            var userId = GetUserId();
            var threat = await _threatService.CreateThreatAlertAsync(
                userId,
                request.ThreatType,
                request.Description,
                Enum.Parse<ThreatSeverity>(request.Severity),
                request.SourceIP,
                request.TargetAsset
            );

            if (threat == null)
                return BadRequest();

            return Ok(threat);
        }
    }

    public class CreateThreatRequest
    {
        public string ThreatType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Severity { get; set; } = "Medium";
        public string SourceIP { get; set; } = string.Empty;
        public string TargetAsset { get; set; } = string.Empty;
    }
}