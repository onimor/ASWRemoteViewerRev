using System.Security.Claims;

namespace ASW.RemoteViewing.Client.Services.UserContext;
 
public interface IUserContextService
{
    Task<ClaimsPrincipal> GetUserAsync();
    Task<string?> GetUserIdAsync();
    Task<string?> GetUsernameAsync();
    Task<string?> GetRoleAsync();
    Task<string?> GetModifiedIdAsync();
    Task<string?> GetAuthSchemeAsync();
    Task<bool> IsInRoleAsync(string role);
    Task<bool> IsAuthenticatedAsync();
}

