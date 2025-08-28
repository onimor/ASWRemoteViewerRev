namespace ASW.RemoteViewing.Features.Authorization.Claims;

public class AuthClaimsInfo
{
    public Guid Id { get; set; }            // Для ClaimTypes.NameIdentifier
    public string Name { get; set; } = "";  // Для ClaimTypes.Name
    public string Role { get; set; } = "";  // Для ClaimTypes.Role
    public Guid ModifiedId { get; set; }    // Для "modified_id"
    public string? AuthScheme { get; set; }
    public Dictionary<string, string>? ExtraClaims { get; set; } // кастомные
}
