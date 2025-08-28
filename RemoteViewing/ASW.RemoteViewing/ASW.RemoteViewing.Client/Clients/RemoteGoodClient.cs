using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.RemoteGood;
using ASW.Shared.Requests.RemoteGood;

namespace ASW.RemoteViewing.Client.Clients;

public class RemoteGoodClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;
    private const string BaseRoute = "/api/v1/ReferenceBooks/Goods"; 
 

    public async Task<List<RemoteGoodDto>?> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetAsync<List<RemoteGoodDto>>(BaseRoute, cancellationToken);
    }

    public async Task<RemoteGoodDto?> GetByIdAsync(Guid goodId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetAsync<RemoteGoodDto>($"{BaseRoute}/{goodId}", cancellationToken);
    }

    public async Task<RemoteGoodDto?> CreateAsync(AddRemoteGoodRequest request, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAsync<AddRemoteGoodRequest, RemoteGoodDto>(BaseRoute, request, cancellationToken);
    }

    public async Task UpdateAsync(UpdateRemoteGoodRequest request, CancellationToken cancellationToken = default)
    {
        await _httpClient.PutAsync(BaseRoute, request, cancellationToken);
    }

    public async Task DeleteAsync(Guid goodId, CancellationToken cancellationToken = default)
    {
        await _httpClient.DeleteAsync($"{BaseRoute}/{goodId}", cancellationToken);
    }
}
