using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;
using BlazingPizza.Services;

namespace BlazingPizza.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequirePinAttribute : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var httpContext = context.HttpContext;
        
        // Check if user is authenticated
        if (!httpContext.User.Identity?.IsAuthenticated == true)
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
            return;
        }

        // Get services
        var pinService = httpContext.RequestServices.GetService<PinService>();
        var pinSessionService = httpContext.RequestServices.GetService<PinSessionService>();

        if (pinService == null || pinSessionService == null)
        {
            await next();
            return;
        }

        // Check PIN status
        var pinStatus = await pinService.GetPinStatusAsync(userId);
        
        // If PIN is not set or not required, allow access
        if (!pinStatus.HasPin || !pinStatus.IsRequired)
        {
            await next();
            return;
        }

        // If PIN is locked, redirect to PIN verification page
        if (pinStatus.IsLocked)
        {
            var returnUrl = httpContext.Request.Path + httpContext.Request.QueryString;
            context.Result = new RedirectResult($"/pin-verification?returnUrl={Uri.EscapeDataString(returnUrl)}");
            return;
        }

        // Check if PIN is verified in session
        var isPinVerified = await pinSessionService.IsPinVerifiedAsync(userId);
        if (!isPinVerified)
        {
            var returnUrl = httpContext.Request.Path + httpContext.Request.QueryString;
            context.Result = new RedirectResult($"/pin-verification?returnUrl={Uri.EscapeDataString(returnUrl)}");
            return;
        }

        // Extend PIN session
        await pinSessionService.ExtendPinSessionAsync(userId);
        
        await next();
    }
} 