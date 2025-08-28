namespace ASW.RemoteViewing.Features.User.Entities;

public class UserEF
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string FullFIO { get; set; } = string.Empty;
    public string ReductionFIO { get; set; } = string.Empty;
    public Guid ModifiedId { get; set; }
    public bool IsRemoved { get; set; }
}
