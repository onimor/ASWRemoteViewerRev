namespace ASW.RemoteViewing.Shared.Dto.User;

public class UserDto
{
    public Guid Id { get; set; }
    public string Login { get; set; } = string.Empty;
    public string FIO { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
