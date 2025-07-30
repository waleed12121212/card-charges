using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Diagnostics;
using BlazingPizza.Shared;

namespace BlazingPizza.Components.Account;

// This is a server-side AuthenticationStateProvider that revalidates the security stamp for the connected user
// every 30 minutes an interactive circuit is connected. It also uses PersistentComponentState to flow the
// authentication state to the client which is then fixed for the lifetime of the WebAssembly application.
internal sealed class PersistingRevalidatingAuthenticationStateProvider : RevalidatingServerAuthenticationStateProvider
{
    private readonly IServiceScopeFactory scopeFactory;
    private readonly PersistentComponentState state;

    private readonly PersistingComponentStateSubscription subscription;

    private Task<AuthenticationState>? authenticationStateTask;

    public PersistingRevalidatingAuthenticationStateProvider(
        ILoggerFactory loggerFactory,
        IServiceScopeFactory serviceScopeFactory,
        PersistentComponentState persistentComponentState)
        : base(loggerFactory)
    {
        scopeFactory = serviceScopeFactory;
        state = persistentComponentState;

        AuthenticationStateChanged += OnAuthenticationStateChanged;
        subscription = state.RegisterOnPersisting(OnPersistingAsync, RenderMode.InteractiveWebAssembly);
    }

    protected override TimeSpan RevalidationInterval => TimeSpan.FromMinutes(30);

    protected override async Task<bool> ValidateAuthenticationStateAsync(
        AuthenticationState authenticationState, CancellationToken cancellationToken)
    {
        // Get the user manager from a new scope to ensure it fetches fresh data
        await using var scope = scopeFactory.CreateAsyncScope();
        return ValidateSecurityStampAsync(authenticationState.User);
    }

    private bool ValidateSecurityStampAsync(ClaimsPrincipal principal)
    {
        if (principal.Identity?.IsAuthenticated != true)
        {
            return false;
        }

        // This validation is very basic and should be enhanced for production scenarios
        // You might want to validate against the database, check if user still exists, etc.
        return true;
    }

    private void OnAuthenticationStateChanged(Task<AuthenticationState> authenticationStateTask)
    {
        this.authenticationStateTask = authenticationStateTask;
    }

    private async Task OnPersistingAsync()
    {
        if (authenticationStateTask is null)
        {
            throw new UnreachableException($"Authentication state not set in {nameof(OnPersistingAsync)}().");
        }

        var authenticationState = await authenticationStateTask;
        var principal = authenticationState.User;

        if (principal.Identity?.IsAuthenticated == true)
        {
            var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var email = principal.FindFirst(ClaimTypes.Name)?.Value ?? 
                       principal.FindFirst(ClaimTypes.Email)?.Value ?? 
                       principal.Identity.Name ?? "";
            var role = principal.FindFirst(ClaimTypes.Role)?.Value ?? "";

            if (!string.IsNullOrEmpty(userId) && !string.IsNullOrEmpty(email))
            {
                state.PersistAsJson("UserInfo", new UserInfo
                {
                    UserId = userId,
                    Email = email,
                });

                Console.WriteLine($"[Server] Persisted UserInfo: UserId={userId}, Email={email}, Role={role}");
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        subscription.Dispose();
        AuthenticationStateChanged -= OnAuthenticationStateChanged;
        base.Dispose(disposing);
    }
} 