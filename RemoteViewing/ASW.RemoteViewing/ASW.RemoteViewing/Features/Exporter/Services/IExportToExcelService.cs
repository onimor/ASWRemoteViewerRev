

using ASW.RemoteViewing.Shared.Requests.RemoteWeighing;

namespace ASW.RemoteViewing.Features.Exporter.Services;

public interface IExportToExcelService
{
    Task<byte[]> GetWeighingsToExcel(ExportWeighingToExcelRequest weighingExportToExcelRequest);
}
