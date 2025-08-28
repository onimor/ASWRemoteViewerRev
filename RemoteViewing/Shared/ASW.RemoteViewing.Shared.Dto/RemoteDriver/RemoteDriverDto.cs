namespace ASW.RemoteViewing.Shared.Dto.RemoteDriver;

public class RemoteDriverDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Comment { get; set; } = string.Empty;
}
