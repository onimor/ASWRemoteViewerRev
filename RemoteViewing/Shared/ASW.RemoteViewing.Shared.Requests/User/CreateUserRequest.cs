namespace ASW.RemoteViewing.Shared.Requests.User;

public class CreateUserRequest
{
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string ReductionFIO { get; set; } = string.Empty;
    public string FullFIO { get; set; } = string.Empty;
}
