using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.RemoteEmptyWeighing;
using ASW.RemoteViewing.Shared.Requests.RemoteWeighing;
using ASW.Shared.Requests.RemoteEmptyWeighing;

namespace ASW.RemoteViewing.Client.Clients;

public class RemoteEmptyWeighingClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;
    private const string BaseRoute = "/api/v1/Remote/Posts/EmptyWeighings";
     

    public async Task<List<RemoteEmptyWeighingDto>?> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetAsync<List<RemoteEmptyWeighingDto>>(BaseRoute, cancellationToken);
    }

    public async Task<List<RemoteEmptyWeighingDto>?> GetByDateAsync(RemoteWeightByDateRequest request, CancellationToken cancellationToken = default)
    {
        var query = $"?DateStart={request.DateStart:O}&DateEnd={request.DateEnd:O}";
        return await _httpClient.GetAsync<List<RemoteEmptyWeighingDto>>(BaseRoute + query, cancellationToken);
    }

    public async Task<List<RemoteEmptyWeighingDto>?> GetByPostAndDateAsync(Guid postId, RemoteWeightByDateRequest request, CancellationToken cancellationToken = default)
    {
        var query = $"?DateStart={request.DateStart:O}&DateEnd={request.DateEnd:O}";
        var route = $"/api/v1/Remote/Posts/{postId}/EmptyWeighings{query}";
        return await _httpClient.GetAsync<List<RemoteEmptyWeighingDto>>(route, cancellationToken);
    }

    public async Task<RemoteEmptyWeighingDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetAsync<RemoteEmptyWeighingDto>($"{BaseRoute}/{id}", cancellationToken);
    }

    public async Task<RemoteEmptyWeighingDto?> CreateAsync(AddRemoteEmptyWeighingRequest request, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAsync<AddRemoteEmptyWeighingRequest, RemoteEmptyWeighingDto>(BaseRoute, request, cancellationToken);
    }

    public async Task UpdateAsync(UpdateRemoteEmptyWeighingRequest request, CancellationToken cancellationToken = default)
    {
        await _httpClient.PutAsync(BaseRoute, request, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _httpClient.DeleteAsync($"{BaseRoute}/{id}", cancellationToken);
    }
}
