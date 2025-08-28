namespace ASW.RemoteViewing.Shared.Dto.RemotePostUsers;

public class RemotePostUserDto
{
    public Guid Id { get; set; } 
    public Guid PlaceId { get; set; }
    public string PlaceName { get; set; } = string.Empty;
    public Guid PostUsersId { get; set; }
    public Guid PostId { get; set; } = Guid.Empty;
    public string PostName { get; set; } = string.Empty;
    public Guid UserId { get; set; } = Guid.Empty;
    public string UserName { get; set; } = string.Empty;
    public bool IsRemoved { get; set; }
}
