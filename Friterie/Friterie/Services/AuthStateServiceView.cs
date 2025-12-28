using Friterie.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Identity.Web;

namespace Friterie.BlazorServer.Services;

public class AuthStateServiceView
{
    private string? _token;
    private User? _currentUser;

    public event Action? OnAuthStateChanged;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_token);

    public User? CurrentUser => _currentUser;


    private readonly AuthenticationStateProvider _auth;

    public AuthStateServiceView(AuthenticationStateProvider auth)
    {
        _auth = auth;
    }

    public async Task<int> GetUserIdAsync()
    {
        if (_currentUser != null)
        {
            return _currentUser.UserId;
        }
        return 0;
    }






    public void Login(string token, User user)
    {
        _token = token;
        _currentUser = user;
        NotifyAuthStateChanged();
    }

    public void Logout()
    {
        _token = null;
        _currentUser = null;
        NotifyAuthStateChanged();
    }

    public string? GetToken() => _token;

    private void NotifyAuthStateChanged() => OnAuthStateChanged?.Invoke();
}


