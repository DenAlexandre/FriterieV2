using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private ClaimsPrincipal _anonymous =
        new ClaimsPrincipal(new ClaimsIdentity());

    private ClaimsPrincipal _currentUser =
        new ClaimsPrincipal(new ClaimsIdentity());

    public override Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        return Task.FromResult(new AuthenticationState(_currentUser));
    }

    public void SignIn(string userName, IEnumerable<Claim>? claims = null)
    {
        var identity = new ClaimsIdentity(
            claims ?? new[]
            {
                new Claim(ClaimTypes.Name, userName)
            },
            authenticationType: "CustomAuth");

        _currentUser = new ClaimsPrincipal(identity);

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(_currentUser)));
    }

    public void SignOut()
    {
        _currentUser = _anonymous;

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(_currentUser)));
    }
}
