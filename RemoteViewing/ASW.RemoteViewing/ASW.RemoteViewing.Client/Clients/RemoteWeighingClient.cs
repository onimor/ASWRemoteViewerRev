using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.RemoteWeighing;
using ASW.RemoteViewing.Shared.Requests.RemoteWeighing;
using ASW.Shared.Requests.RemoteWeighing;

namespace ASW.RemoteViewing.Client.Clients;

public class RemoteWeighingClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;
    private const string BaseRoute = "/api/v1/Remote/Posts/Weighings";

    public async Task<List<RemoteWeighingDto>?> GetAllAsync()
    {
        return await _httpClient.GetAsync<List<RemoteWeighingDto>>(BaseRoute);
    }

    public async Task<RemoteWeighingDto?> GetByIdAsync(Guid weighingId)
    {
        return await _httpClient.GetAsync<RemoteWeighingDto>($"{BaseRoute}/{weighingId}");
    }

    public async Task CreateAsync(AddRemoteWeighingRequest request)
    {
        await _httpClient.PostAsync(BaseRoute, request);
    }

    public async Task UpdateAsync(Guid weighingId, UpdateRemoteWeighingRequest request)
    {
        await _httpClient.PutAsync($"{BaseRoute}/{weighingId}", request);
    }

    public async Task DeleteAsync(Guid weighingId)
    {
        await _httpClient.DeleteAsync($"{BaseRoute}/{weighingId}");
    }

    public async Task<List<RemoteWeighingDto>?> GetByPostAndDateAsync(Guid postId, RemoteWeightByDateRequest request)
    {
        var query = BuildQuery(request);
        return await _httpClient.GetAsync<List<RemoteWeighingDto>>($"/api/v1/Remote/Posts/{postId}/Weighings{query}");
    }

    public async Task<RemoteWeighingDto?> GetLastWeighingsAsync(Guid postId)
    {
        return await _httpClient.GetAsync<RemoteWeighingDto>($"/api/v1/Remote/Posts/{postId}/Weighings/Last");
    }

    public async Task<List<RemoteWeighingDto>?> GetByDateAsync(RemoteWeightByDateRequest request)
    {
        var query = BuildQuery(request);
        return await _httpClient.GetAsync<List<RemoteWeighingDto>>($"{BaseRoute}/ByDate{query}");
    }

    private static string BuildQuery(RemoteWeightByDateRequest request)
    {
        var query = System.Web.HttpUtility.ParseQueryString(string.Empty);
        if (request.DateStart.HasValue)
            query["DateStart"] = request.DateStart.Value.ToUniversalTime().ToString("o");
        if (request.DateEnd.HasValue)
            query["DateEnd"] = request.DateEnd.Value.ToUniversalTime().ToString("o");

        return "?" + query.ToString();
    }
}
