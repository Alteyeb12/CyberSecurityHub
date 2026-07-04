using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace CyberInsight.Middleware
{
    public class AuthenticationMiddleware : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User;
            if (user?.Identity?.IsAuthenticated == true)
            {
                // تحديث آخر نشاط للمستخدم
                context.HttpContext.Items["LastActivity"] = DateTime.UtcNow;
            }

            await next();
        }
    }

    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync(new { error = "An error occurred" });
            }
        }
    }

    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;

        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var startTime = DateTime.UtcNow;
            var path = context.Request.Path;
            var method = context.Request.Method;

            _logger.LogInformation($"Request: {method} {path} started at {startTime}");

            await _next(context);

            var duration = DateTime.UtcNow - startTime;
            _logger.LogInformation($"Request: {method} {path} completed in {duration.TotalMilliseconds}ms");
        }
    }
}
