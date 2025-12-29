namespace Friterie.API.Services;

using System.Security.Claims;

public class AuthStateService
{
    public ClaimsPrincipal? User { get; private set; }
    public string? UserId { get; private set; }
    public string? Token { get; private set; }

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated == true;

    public void SetUser(ClaimsPrincipal user, string token)
    {
        User = user;
        Token = token;
        UserId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public void Clear()
    {
        User = null;
        Token = null;
        UserId = null;
    }
}
