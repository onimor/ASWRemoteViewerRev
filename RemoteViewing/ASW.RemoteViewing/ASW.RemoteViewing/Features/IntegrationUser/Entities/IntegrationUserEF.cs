using ASW.RemoteViewing.Shared.Constants;

namespace ASW.RemoteViewing.Features.IntegrationUser.Entities;

public class IntegrationUserEF
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = Roles.Integration;
    public string KeyHash { get; set; } = string.Empty;
    public string KeyPrefix { get; set; } = string.Empty;
    public Guid? GeneratedByUserId { get; set; }
    public string? GeneratedByUserName { get; set; }
    public DateTime? KeyGeneratedAt { get; set; } 
    public bool IsRemoved { get; set; } 
}
