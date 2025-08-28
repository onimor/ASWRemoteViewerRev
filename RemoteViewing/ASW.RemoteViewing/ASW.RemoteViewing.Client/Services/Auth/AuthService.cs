using ASW.RemoteViewing.Client.Clients;
using ASW.RemoteViewing.Client.Infrastructure.Providers;
using ASW.RemoteViewing.Client.Services.Token;
using Microsoft.AspNetCore.Components.Authorization;

namespace ASW.RemoteViewing.Client.Services.Auth;

public class AuthService : IAuthService
{
    private readonly ITokenProvider _tokenProvider;
    private readonly CustomAuthenticationStateProvider _authStateProvider;
    private readonly AuthClient _authClient;
     
    public AuthService(
        ITokenProvider tokenProvider, 
        AuthenticationStateProvider provider,
        AuthClient authClient)
    {
        _tokenProvider = tokenProvider;
        _authStateProvider = (CustomAuthenticationStateProvider)provider;
        _authClient = authClient;
    }

    public async Task<bool> SignInAsync(string username, string password, bool isRememberMe)
    {
        var response = await _authClient.LoginAsync(username, password);
        if (response is null) return false;
        await _tokenProvider.SetTokenAsync(response.Token, isRememberMe);
        _authStateProvider.MarkUserAsAuthenticated(response.Token);
        return true;
    }

    public async Task SignOutAsync()
    {
        await _tokenProvider.RemoveTokenAsync();
        _authStateProvider.MarkUserAsLoggedOut();
    }

    public async Task<string?> GetTokenAsync() 
    {
       return await _tokenProvider.GetTokenAsync();
    }
}
