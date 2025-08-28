namespace ASW.RemoteViewing.Shared.Dto.RemotePhoto;

public class RemotePhotoTaraDto
{
    public Guid Id { get; set; }
    public Guid PlaceId { get; set; }
    public string PlaceName { get; set; } = string.Empty;
    public Guid PhotoTaraId { get; set; }
    public Guid WeighingId { get; set; } = Guid.Empty;
    public DateTime? Date { get; set; }
    public string Base64Image { get; set; } = string.Empty; 
}
