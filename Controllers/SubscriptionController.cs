using CyberInsight.Models;
using CyberInsight.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CyberInsight.Controllers
{
    [Authorize(AuthenticationSchemes = "CyberInsightScheme")]
    public class SubscriptionController : Controller
    {
        private readonly IStripeService _stripeService;
        private readonly ILogger<SubscriptionController> _logger;

        public SubscriptionController(IStripeService stripeService, ILogger<SubscriptionController> logger)
        {
            _stripeService = stripeService;
            _logger = logger;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
        }

        public IActionResult Plans()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(string plan)
        {
            var userId = GetUserId();
            var successUrl = $"{Request.Scheme}://{Request.Host}/subscription/success";
            var cancelUrl = $"{Request.Scheme}://{Request.Host}/subscription/plans";

            var checkoutUrl = await _stripeService.GetCheckoutSessionAsync(userId, plan, successUrl, cancelUrl);
            if (checkoutUrl == null)
                return RedirectToAction(nameof(Plans));

            return Redirect(checkoutUrl);
        }

        public IActionResult Success()
        {
            ViewBag.Message = "Subscription updated successfully!";
            return View();
        }
    }
}