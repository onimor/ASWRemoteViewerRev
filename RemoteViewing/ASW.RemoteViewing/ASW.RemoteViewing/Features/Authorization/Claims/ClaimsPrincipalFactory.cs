using ASW.RemoteViewing.Features.Authorization.Claims;
using System.Security.Claims;

public static class ClaimsPrincipalFactory
{
    public static IEnumerable<Claim> CreateClaims(AuthClaimsInfo info)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, info.Id.ToString()),
            new(ClaimTypes.Name, info.Name),
            new(ClaimTypes.Role, info.Role),
            new("modified_id", info.ModifiedId.ToString())
        };

        if (string.IsNullOrWhiteSpace(info.AuthScheme) == false)
            claims.Add(new("auth_scheme", info.AuthScheme));

        if (info.ExtraClaims is not null)
        {
            foreach (var kvp in info.ExtraClaims)
                claims.Add(new(kvp.Key, kvp.Value));
        }

        return claims;
    }
}