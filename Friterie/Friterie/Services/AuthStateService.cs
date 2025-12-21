namespace Friterie.BlazorServer.Services;

public class AuthStateService
{
    private string? _token;
    private UserInfoServer? _currentUser;

    public event Action? OnAuthStateChanged;

    public bool IsAuthenticated => !string.IsNullOrEmpty(_token);

    public UserInfoServer? CurrentUser => _currentUser;

    public void Login(string token, UserInfoServer user)
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


