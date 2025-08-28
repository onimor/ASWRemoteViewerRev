namespace ASW.RemoteViewing.Shared.Dto.User;

public class UserInfoDto
{
    public Guid Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string FullFIO { get; set; } = string.Empty;
    public string ReductionFIO { get; set; } = string.Empty;
    public Guid ModifiedId { get; set; }
    public bool IsOnline { get; set; }

}
