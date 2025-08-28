namespace ASW.RemoteViewing.Client.Services.Auth;

public interface IAuthService
{
    Task<bool> SignInAsync(string username, string password, bool isRememberMe);
    Task SignOutAsync();
    Task<string?> GetTokenAsync();
}
