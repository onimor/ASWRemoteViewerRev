namespace ASW.RemoteViewing.Shared.Dto.RemoteEmptyWeighing;

public class RemoteEmptyWeighingDto
{
    public Guid Id { get; set; } 
    public Guid PostId { get; set; }
    public string? PostName { get; set; }
    public string? CarName { get; set; }
    public string? TrailerName { get; set; }
    public DateTime? Date { get; set; }
    public double Weight { get; set; }
    public bool Stability { get; set; }
    public bool IsRemoved { get; set; }
}
