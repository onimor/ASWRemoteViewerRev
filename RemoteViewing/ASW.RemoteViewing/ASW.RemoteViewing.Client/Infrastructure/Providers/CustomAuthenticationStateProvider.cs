using ASW.RemoteViewing.Client.Services.Token;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ASW.RemoteViewing.Client.Infrastructure.Providers;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly ClaimsPrincipal _anonymous = new(new ClaimsIdentity());
    private readonly ITokenProvider _tokenProvider;
    private AuthenticationState? _cachedAuthenticationState;
    public CustomAuthenticationStateProvider(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (_cachedAuthenticationState != null)
            return _cachedAuthenticationState;

        var token = await _tokenProvider.GetTokenAsync();

        if (string.IsNullOrWhiteSpace(token))
            return _cachedAuthenticationState = new AuthenticationState(_anonymous);

        var userInfo = await _tokenProvider.ValidateTokenAsync(token);
        if (userInfo == null)
        {
            await _tokenProvider.RemoveTokenAsync();
            return _cachedAuthenticationState = new AuthenticationState(_anonymous);
        } 

        var claims = ParseClaimsFromJwt(token);
        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

        return _cachedAuthenticationState = new AuthenticationState(user);
    }

    public void MarkUserAsAuthenticated(string token)
    {
        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "apiauth");
        var user = new ClaimsPrincipal(identity);
        _cachedAuthenticationState = new AuthenticationState(user);
        NotifyAuthenticationStateChanged(Task.FromResult(_cachedAuthenticationState));
    }

    public void MarkUserAsLoggedOut()
    {
        _cachedAuthenticationState = new AuthenticationState(_anonymous);
        NotifyAuthenticationStateChanged(Task.FromResult(_cachedAuthenticationState));
    }

    private List<Claim> ParseClaimsFromJwt(string jwt)
    {
        // Можно использовать JwtSecurityTokenHandler или просто вручную парсить payload
        var claims = new List<Claim>();
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        claims.AddRange(token.Claims);
        return claims;
    }
}