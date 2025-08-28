using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.RemoteAxes;
using ASW.Shared.Requests.RemoteAxesWeight;

namespace ASW.RemoteViewing.Client.Clients;

public class RemoteAxesWeightClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;

    private static string GetRoute(Guid weighingId) =>
        $"/api/v1/Remote/Posts/Weighing/{weighingId}/Axes/Weight";

    public async Task<List<RemoteAxesWeightDto>?> GetByWeighingAsync(Guid weighingId, CancellationToken cancellationToken = default)
    {
        var url = GetRoute(weighingId);
        return await _httpClient.GetAsync<List<RemoteAxesWeightDto>>(url, cancellationToken);
    }

    public async Task CreateAsync(AddRemoteAxesWeightRequest request, CancellationToken cancellationToken = default)
    {
        var url = GetRoute(request.WeighingId);
        await _httpClient.PostAsync(url, request, cancellationToken);
    }

    public async Task DeleteAsync(Guid weighingId, CancellationToken cancellationToken = default)
    {
        var url = GetRoute(weighingId);
        await _httpClient.DeleteAsync(url, cancellationToken);
    }
}
