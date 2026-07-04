namespace CyberInsight.Models
{
    public enum ThreatSeverity
    {
        Low,
        Medium,
        High,
        Critical
    }

    public class ThreatAlert
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ThreatType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public ThreatSeverity Severity { get; set; } = ThreatSeverity.Medium;
        public string SourceIP { get; set; } = string.Empty;
        public string TargetAsset { get; set; } = string.Empty;
        public DateTime DetectedAt { get; set; } = DateTime.UtcNow;
        public bool IsResolved { get; set; } = false;
        public string? ResolutionNotes { get; set; }
        public DateTime? ResolvedAt { get; set; }
        public int AlertCount { get; set; } = 1;
        public string? ThreatIntelligence { get; set; }

        // Navigation property
        public User User { get; set; } = null!;
    }
}