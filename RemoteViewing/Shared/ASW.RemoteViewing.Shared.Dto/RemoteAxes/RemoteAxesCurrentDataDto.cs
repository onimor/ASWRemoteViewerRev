namespace ASW.RemoteViewing.Shared.Dto.RemoteAxes;

public class RemoteAxesCurrentDataDto
{
    public double Weight { get; set; }
    public int Stability { get; set; }
    public bool IsCompletedWeighing  { get; set; }
    public RemoteAxesDto? Axes { get; set; }
}
