using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ASW.RemoteViewing.Client.Services.UserContext;

public class UserContextService : IUserContextService
{
    private readonly AuthenticationStateProvider _authProvider;
    private ClaimsPrincipal? _cachedUser;

    public UserContextService(AuthenticationStateProvider authProvider)
    {
        _authProvider = authProvider;
    } 

    public async Task<ClaimsPrincipal> GetUserAsync()
    {
        if (_cachedUser is not null)
            return _cachedUser;

        var authState = await _authProvider.GetAuthenticationStateAsync();
        _cachedUser = authState.User;
        return _cachedUser;
    }

    public async Task<string?> GetUserIdAsync()
    {
        var user = await GetUserAsync();
        return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }

    public async Task<string?> GetUsernameAsync()
    {
        var user = await GetUserAsync();
        return user.FindFirst(ClaimTypes.Name)?.Value;
    }

    public async Task<string?> GetRoleAsync()
    {
        var user = await GetUserAsync();
        return user.FindFirst(ClaimTypes.Role)?.Value;
    }

    public async Task<string?> GetModifiedIdAsync()
    {
        var user = await GetUserAsync();
        return user.FindFirst("modified_id")?.Value;
    }

    public async Task<string?> GetAuthSchemeAsync()
    {
        var user = await GetUserAsync();
        return user.FindFirst("auth_scheme")?.Value;
    }

    public async Task<bool> IsInRoleAsync(string role)
    {
        var user = await GetUserAsync();
        return user.IsInRole(role);
    }
    public async Task<bool> IsAuthenticatedAsync()
    {
        var user = await GetUserAsync();
        return user.Identity?.IsAuthenticated ?? false;
    }
}