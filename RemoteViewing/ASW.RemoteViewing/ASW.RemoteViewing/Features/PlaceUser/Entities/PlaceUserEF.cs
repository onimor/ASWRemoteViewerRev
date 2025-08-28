using ASW.RemoteViewing.Shared.Constants;

namespace ASW.RemoteViewing.Features.PlaceUser.Entities;

public class PlaceUserEF
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty; 
    public string Role { get; set; } = Roles.PlaceClient;  
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid ModifiedId { get; set; }
    public Guid? ModifiedByUserId { get; set; }
    public string? ModifiedByUserName { get; set; } = string.Empty;
    public DateTime ModifiedAt { get; set; }
    public bool IsRemoved { get; set; }
}
