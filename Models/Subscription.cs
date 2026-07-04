namespace CyberInsight.Models
{
    public enum SubscriptionPlan
    {
        Free,
        Professional,
        Enterprise
    }

    public enum SubscriptionStatus
    {
        Active,
        Inactive,
        Suspended,
        Cancelled
    }

    public class Subscription
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public SubscriptionPlan Plan { get; set; } = SubscriptionPlan.Free;
        public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Active;
        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime? EndDate { get; set; }
        public decimal Price { get; set; }
        public string? StripeCustomerId { get; set; }
        public string? StripeSubscriptionId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation property
        public User User { get; set; } = null!;
    }
}