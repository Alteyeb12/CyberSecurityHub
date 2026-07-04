using CyberInsight.Data;
using CyberInsight.Models;
using Microsoft.EntityFrameworkCore;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;

namespace CyberInsight.Services
{
    public class StripeService : IStripeService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<StripeService> _logger;
        private readonly IConfiguration _configuration;

        public StripeService(ApplicationDbContext context, ILogger<StripeService> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;
            StripeConfiguration.ApiKey = _configuration["Stripe:SecretKey"];
        }

        public async Task<string?> CreateCustomerAsync(string email, string name)
        {
            try
            {
                var options = new CustomerCreateOptions
                {
                    Email = email,
                    Name = name
                };

                var service = new CustomerService();
                var customer = await service.CreateAsync(options);
                return customer.Id;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Stripe customer creation error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> CreateSubscriptionAsync(int userId, string plan, decimal amount)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                var subscription = await _context.Subscriptions
                    .FirstOrDefaultAsync(s => s.UserId == userId);

                if (subscription != null)
                {
                    subscription.Plan = Enum.Parse<SubscriptionPlan>(plan);
                    subscription.Price = amount;
                    subscription.Status = SubscriptionStatus.Active;
                    subscription.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Subscription created for user {userId}: {plan}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Subscription creation error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> CancelSubscriptionAsync(int userId)
        {
            try
            {
                var subscription = await _context.Subscriptions
                    .FirstOrDefaultAsync(s => s.UserId == userId && s.Status == SubscriptionStatus.Active);

                if (subscription == null) return false;

                subscription.Status = SubscriptionStatus.Cancelled;
                subscription.EndDate = DateTime.UtcNow;
                subscription.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                _logger.LogInformation($"Subscription cancelled for user {userId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Subscription cancellation error: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ProcessWebhookAsync(string json, string signature)
        {
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    signature,
                    _configuration["Stripe:WebhookSecret"] ?? string.Empty
                );

                switch (stripeEvent.Type)
                {
                    case "customer.subscription.updated":
                    case "customer.subscription.created":
                        _logger.LogInformation("Subscription webhook processed");
                        break;
                    case "customer.subscription.deleted":
                        _logger.LogInformation("Subscription deleted webhook processed");
                        break;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Webhook processing error: {ex.Message}");
                return false;
            }
        }

        public async Task<string?> GetCheckoutSessionAsync(int userId, string plan, string successUrl, string cancelUrl)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return null;

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string> { "card" },
                    LineItems = new List<SessionLineItemOptions>
                    {
                        new SessionLineItemOptions
                        {
                            PriceData = new SessionLineItemPriceDataOptions
                            {
                                UnitAmount = GetPlanAmount(plan),
                                Currency = "usd",
                                ProductData = new SessionLineItemPriceDataProductDataOptions
                                {
                                    Name = $"CyberInsight {plan} Plan",
                                }
                            },
                            Quantity = 1,
                        }
                    },
                    Mode = "subscription",
                    SuccessUrl = successUrl,
                    CancelUrl = cancelUrl,
                    CustomerEmail = user.Email,
                    Metadata = new Dictionary<string, string>
                    {
                        { "userId", userId.ToString() },
                        { "plan", plan }
                    }
                };

                var service = new SessionService();
                var session = await service.CreateAsync(options);
                return session.Url;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Checkout session creation error: {ex.Message}");
                return null;
            }
        }

        private long GetPlanAmount(string plan)
        {
            return plan.ToLower() switch
            {
                "professional" => 99_00, // $99 per month
                "enterprise" => 299_00, // $299 per month
                _ => 0 // Free
            };
        }
    }
}