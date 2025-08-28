namespace ASW.RemoteViewing.Shared.Requests.User;

public class UpdateUserRequest
{
    public string Login { get; set; } = string.Empty;
    public bool IsNeedNewPassword { get; set; }
    public string Password { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string FIO { get; set; } = string.Empty; 
}
