using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.RemoteAxes;
using ASW.Shared.Requests.RemoteAxesDist;

namespace ASW.RemoteViewing.Client.Clients;

public class RemoteAxesDistClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;

    private static string GetRoute(Guid weighingId) =>
        $"/api/v1/Remote/Posts/Weighing/{weighingId}/Axes/Dist";

    public async Task<RemoteAxesDistDto?> GetByWeighingAsync(Guid weighingId, CancellationToken cancellationToken = default)
    {
        var url = GetRoute(weighingId);
        return await _httpClient.GetAsync<RemoteAxesDistDto>(url, cancellationToken);
    }

    public async Task<RemoteAxesDistDto?> CreateAsync(AddRemoteAxesDistRequest request, CancellationToken cancellationToken = default)
    {
        var url = GetRoute(request.WeighingId);
        return await _httpClient.PostAsync<AddRemoteAxesDistRequest, RemoteAxesDistDto>(url, request, cancellationToken);
    }

    public async Task DeleteAsync(Guid weighingId, CancellationToken cancellationToken = default)
    {
        var url = GetRoute(weighingId);
        await _httpClient.DeleteAsync(url, cancellationToken);
    }
}
