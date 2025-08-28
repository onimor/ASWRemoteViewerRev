using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.RemotePost;
using ASW.Shared.Requests.RemotePost;

namespace ASW.RemoteViewing.Client.Clients;

public class RemotePostClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;
    private const string BaseRoute = "/api/v1/Posts";

    public async Task<List<RemotePostDto>?> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetAsync<List<RemotePostDto>>(BaseRoute, cancellationToken);
    }

    public async Task<RemotePostDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetAsync<RemotePostDto>($"{BaseRoute}/{id}", cancellationToken);
    }

    public async Task<AddRemotePostRequest?> CreateAsync(AddRemotePostRequest request, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAsync<AddRemotePostRequest, AddRemotePostRequest>($"{BaseRoute}", request, cancellationToken);
    }

    public async Task UpdateAsync(UpdateRemotePostRequest request, CancellationToken cancellationToken = default)
    {
        await _httpClient.PutAsync($"{BaseRoute}", request, cancellationToken);
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _httpClient.DeleteAsync($"{BaseRoute}/{id}", cancellationToken);
    }
}
