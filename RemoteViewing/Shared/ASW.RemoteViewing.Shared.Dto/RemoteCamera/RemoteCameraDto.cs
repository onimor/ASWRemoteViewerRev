namespace ASW.RemoteViewing.Shared.Dto.RemoteCamera;

public class RemoteCameraDto
{
    public Guid PlaceId { get; set; }
    public string PlaceName { get; set; } = string.Empty;
    public Guid CameraId { get; set; }
    public Guid PostId { get; set; } = Guid.Empty;
    public string Name { get; set; } = string.Empty;
    public string URL { get; set; } = string.Empty;
    public int SequenceNumber { get; set; }
    public bool IsRecognize { get; set; }
    public double X { get; set; } = 0;
    public double Y { get; set; } = 0;
    public double Z { get; set; } = 0; 
}
