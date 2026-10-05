using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace TravelBD.Api.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AdminApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string ApiKeyHeaderName = "X-Admin-Api-Key";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var configuredKey = configuration.GetValue<string>("AdminApiKey");

        // If no key is configured in environment/settings, fallback to a default dev key
        configuredKey = string.IsNullOrWhiteSpace(configuredKey) ? "travelbd-admin-secret-2026" : configuredKey;

        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey)
            || !string.Equals(extractedApiKey, configuredKey, StringComparison.Ordinal))
        {
            context.Result = new UnauthorizedObjectResult(new { message = "Unauthorized: Invalid or missing Admin API Key." });
            return;
        }

        await next();
    }
}
