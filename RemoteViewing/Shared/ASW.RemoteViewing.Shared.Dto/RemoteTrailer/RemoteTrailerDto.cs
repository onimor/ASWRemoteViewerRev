namespace ASW.RemoteViewing.Shared.Dto.RemoteTrailer;

public class RemoteTrailerDto
{
    public Guid Id { get; set; }
    public string GovernmentNumber { get; set; } = string.Empty;
    public string Brand { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
}
