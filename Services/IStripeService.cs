namespace CyberInsight.Services
{
    public interface IStripeService
    {
        Task<string?> CreateCustomerAsync(string email, string name);
        Task<bool> CreateSubscriptionAsync(int userId, string plan, decimal amount);
        Task<bool> CancelSubscriptionAsync(int userId);
        Task<bool> ProcessWebhookAsync(string json, string signature);
        Task<string?> GetCheckoutSessionAsync(int userId, string plan, string successUrl, string cancelUrl);
    }
}