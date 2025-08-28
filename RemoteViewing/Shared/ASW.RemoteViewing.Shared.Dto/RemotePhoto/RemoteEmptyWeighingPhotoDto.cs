namespace ASW.RemoteViewing.Shared.Dto.RemotePhoto;

public class RemoteEmptyWeighingPhotoDto
{
    public Guid Id { get; set; }
    public Guid PlaceId { get; set; }
    public string PlaceName { get; set; } = string.Empty;
    public Guid EmptyWeighingPhotoId { get; set; }
    public Guid EmptyWeighingId { get; set; } = Guid.Empty;
    public DateTime? Date { get; set; }
    public string Base64Image { get; set; } = string.Empty;
    public bool IsRemoved { get; set; }
}
