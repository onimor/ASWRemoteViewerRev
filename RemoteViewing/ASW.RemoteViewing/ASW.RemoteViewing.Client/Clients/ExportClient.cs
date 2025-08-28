using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Requests.RemoteWeighing;

namespace ASW.RemoteViewing.Client.Clients;

public class ExportClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;
     
     
    public async Task<byte[]?> ExportRemoteWeighingsToExcel(ExportWeighingToExcelRequest exportWeighingToExcelRequest)
    {
        return await _httpClient.PostAsyncForFile("/api/v1/Remote/Posts/Weighings/Export/Excel", exportWeighingToExcelRequest);
    }
}
