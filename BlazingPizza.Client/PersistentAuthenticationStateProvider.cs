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
        if (!state.TryTakeFromJson<UserInfo>(nameof(UserInfo) , out var userInfo) || userInfo is null)
        {
            return;
        }

        Claim[] claims = [
            new Claim(ClaimTypes.NameIdentifier, userInfo.UserId),
            new Claim(ClaimTypes.Name, userInfo.Email),
            new Claim(ClaimTypes.Email, userInfo.Email) ];

        authenticationStateTask = Task.FromResult(
            new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(claims ,
                authenticationType: nameof(PersistentAuthenticationStateProvider)))));
    }

    public override Task<AuthenticationState> GetAuthenticationStateAsync() => authenticationStateTask;

    public void SetUnauthenticated()
    {
        authenticationStateTask = defaultUnauthenticatedTask;
        NotifyAuthenticationStateChanged(authenticationStateTask);
    }
}
