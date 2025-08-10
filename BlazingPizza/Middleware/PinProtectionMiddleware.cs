using System.Security.Claims;
using BlazingPizza.Services;

namespace BlazingPizza.Middleware;

public class PinProtectionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string[] _protectedPaths = {
        "/admin/dashboard",
        "/digital-goods",
        "/recharge",
        "/internet-packages",
        "/account/manage"
    };

    public PinProtectionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var path = context.Request.Path.Value?.ToLowerInvariant();
        
        // Check if this path requires PIN protection
        var requiresPin = _protectedPaths.Any(protectedPath => 
            path?.StartsWith(protectedPath, StringComparison.OrdinalIgnoreCase) == true);

        if (!requiresPin)
        {
            await _next(context);
            return;
        }

        // Check if user is authenticated
        if (!context.User.Identity?.IsAuthenticated == true)
        {
            context.Response.Redirect("/Account/Login");
            return;
        }

        var userId = context.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            context.Response.Redirect("/Account/Login");
            return;
        }

        // Get services
        var pinService = context.RequestServices.GetService<PinService>();
        var pinSessionService = context.RequestServices.GetService<PinSessionService>();

        if (pinService == null || pinSessionService == null)
        {
            await _next(context);
            return;
        }

        try
        {
            // Check PIN status
            var pinStatus = await pinService.GetPinStatusAsync(userId);
            
            // If PIN is not set or not required, allow access
            if (!pinStatus.HasPin || !pinStatus.IsRequired)
            {
                await _next(context);
                return;
            }

            // If PIN is locked, redirect to PIN verification page
            if (pinStatus.IsLocked)
            {
                var returnUrl = context.Request.Path + context.Request.QueryString;
                context.Response.Redirect($"/pin-verification?returnUrl={Uri.EscapeDataString(returnUrl)}");
                return;
            }

            // Check if PIN is verified in session
            var isPinVerified = await pinSessionService.IsPinVerifiedAsync(userId);
            if (!isPinVerified)
            {
                var returnUrl = context.Request.Path + context.Request.QueryString;
                context.Response.Redirect($"/pin-verification?returnUrl={Uri.EscapeDataString(returnUrl)}");
                return;
            }

            // Extend PIN session
            await pinSessionService.ExtendPinSessionAsync(userId);
        }
        catch (Exception)
        {
            // Log error and continue - don't break the application
            // You might want to add proper logging here
        }

        await _next(context);
    }
} 