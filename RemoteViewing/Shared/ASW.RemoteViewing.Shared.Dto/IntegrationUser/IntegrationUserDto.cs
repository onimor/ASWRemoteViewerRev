namespace ASW.RemoteViewing.Shared.Dto.IntegrationUser;

public class IntegrationUserDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string KeyHash { get; set; } = string.Empty;
    public string KeyPrefix { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
