using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace BlazingPizza.Client;

internal class PersistentAuthenticationStateProvider : AuthenticationStateProvider
{
    private static readonly Task<AuthenticationState> defaultUnauthenticatedTask =
        Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

    private Task<AuthenticationState> authenticationStateTask = defaultUnauthenticatedTask;

    public PersistentAuthenticationStateProvider(PersistentComponentState state)
    {
        Console.WriteLine("[Client] Initializing PersistentAuthenticationStateProvider");

        // استخدام نفس الـ key المستخدم في الـ server
        if (!state.TryTakeFromJson<UserInfo>("UserInfo" , out var userInfo) || userInfo is null)
        {
            Console.WriteLine("[Client] No UserInfo found in persistent state");
            return;
        }

        Console.WriteLine($"[Client] Found UserInfo: UserId={userInfo.UserId}, Email={userInfo.Email}");

        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
            new Claim(ClaimTypes.Name, userInfo.Email),
            new Claim(ClaimTypes.Email, userInfo.Email)
        ];

        authenticationStateTask = Task.FromResult(
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims ,
                authenticationType: nameof(PersistentAuthenticationStateProvider)))));

        Console.WriteLine("[Client] Authentication state set successfully in WebAssembly");
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync( ) => authenticationStateTask;

    public void SetUnauthenticated( )
    {
        authenticationStateTask = defaultUnauthenticatedTask;
        NotifyAuthenticationStateChanged(authenticationStateTask);
    }
}