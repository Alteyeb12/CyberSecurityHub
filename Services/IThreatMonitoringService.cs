using CyberInsight.Models;

namespace CyberInsight.Services
{
    public interface IThreatMonitoringService
    {
        Task<List<ThreatAlert>> GetUserThreatsAsync(int userId);
        Task<ThreatAlert?> CreateThreatAlertAsync(int userId, string threatType, string description, ThreatSeverity severity, string sourceIp, string targetAsset);
        Task<bool> ResolveThreatAsync(int alertId, string resolutionNotes);
        Task<List<ThreatAlert>> GetCriticalThreatsAsync(int userId);
        Task<Dictionary<string, int>> GetThreatStatisticsAsync(int userId);
    }
}