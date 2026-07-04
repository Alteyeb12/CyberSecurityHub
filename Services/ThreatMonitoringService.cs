using CyberInsight.Data;
using CyberInsight.Models;
using Microsoft.EntityFrameworkCore;

namespace CyberInsight.Services
{
    public class ThreatMonitoringService : IThreatMonitoringService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ThreatMonitoringService> _logger;

        public ThreatMonitoringService(ApplicationDbContext context, ILogger<ThreatMonitoringService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<ThreatAlert>> GetUserThreatsAsync(int userId)
        {
            try
            {
                return await _context.ThreatAlerts
                    .Where(t => t.UserId == userId)
                    .OrderByDescending(t => t.DetectedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching threats: {ex.Message}");
                return new List<ThreatAlert>();
            }
        }

        public async Task<ThreatAlert?> CreateThreatAlertAsync(int userId, string threatType, string description, ThreatSeverity severity, string sourceIp, string targetAsset)
        {
            try
            {
                var existingThreat = await _context.ThreatAlerts
                    .FirstOrDefaultAsync(t => t.UserId == userId && 
                                            t.SourceIP == sourceIp && 
                                            t.ThreatType == threatType && 
                                            !t.IsResolved);

                if (existingThreat != null)
                {
                    existingThreat.AlertCount++;
                    existingThreat.DetectedAt = DateTime.UtcNow;
                    await _context.SaveChangesAsync();
                    return existingThreat;
                }

                var alert = new ThreatAlert
                {
                    UserId = userId,
                    ThreatType = threatType,
                    Description = description,
                    Severity = severity,
                    SourceIP = sourceIp,
                    TargetAsset = targetAsset,
                    DetectedAt = DateTime.UtcNow,
                    AlertCount = 1
                };

                _context.ThreatAlerts.Add(alert);
                await _context.SaveChangesAsync();
                _logger.LogInformation($"Threat alert created for user {userId}: {threatType}");
                return alert;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating threat alert: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> ResolveThreatAsync(int alertId, string resolutionNotes)
        {
            try
            {
                var alert = await _context.ThreatAlerts.FindAsync(alertId);
                if (alert == null) return false;

                alert.IsResolved = true;
                alert.ResolutionNotes = resolutionNotes;
                alert.ResolvedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Threat alert {alertId} resolved");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error resolving threat: {ex.Message}");
                return false;
            }
        }

        public async Task<List<ThreatAlert>> GetCriticalThreatsAsync(int userId)
        {
            try
            {
                return await _context.ThreatAlerts
                    .Where(t => t.UserId == userId && 
                               t.Severity == ThreatSeverity.Critical && 
                               !t.IsResolved)
                    .OrderByDescending(t => t.DetectedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching critical threats: {ex.Message}");
                return new List<ThreatAlert>();
            }
        }

        public async Task<Dictionary<string, int>> GetThreatStatisticsAsync(int userId)
        {
            try
            {
                var threats = await _context.ThreatAlerts
                    .Where(t => t.UserId == userId)
                    .GroupBy(t => t.ThreatType)
                    .Select(g => new { Type = g.Key, Count = g.Count() })
                    .ToListAsync();

                return threats.ToDictionary(t => t.Type, t => t.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error fetching statistics: {ex.Message}");
                return new Dictionary<string, int>();
            }
        }
    }
}