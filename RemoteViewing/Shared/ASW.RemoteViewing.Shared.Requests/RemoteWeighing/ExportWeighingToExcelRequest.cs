using ASW.RemoteViewing.Shared.Dto.ExportToExcel;

namespace ASW.RemoteViewing.Shared.Requests.RemoteWeighing;

public class ExportWeighingToExcelRequest
{
    public DateTime? DateStart { get; set; }
    public DateTime? DateEnd { get; set; }
    public string WeighingStage { get; set; } = string.Empty;
    public Guid PostId { get; set; }
    public string PostName { get; set; } = string.Empty;
    public ExportToExcelFiltersDto? Filters { get; set; }

}
