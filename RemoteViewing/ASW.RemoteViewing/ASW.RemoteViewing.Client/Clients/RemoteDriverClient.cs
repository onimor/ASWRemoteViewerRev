using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.RemoteDriver;
using ASW.Shared.Requests.RemoteDriver;

namespace ASW.RemoteViewing.Client.Clients;

public class RemoteDriverClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;
    private const string BaseRoute = "/api/v1/ReferenceBooks/Drivers";
     

    public async Task<List<RemoteDriverDto>?> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetAsync<List<RemoteDriverDto>>(BaseRoute, cancellationToken);
    }

    public async Task<RemoteDriverDto?> GetByIdAsync(Guid driverId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetAsync<RemoteDriverDto>($"{BaseRoute}/{driverId}", cancellationToken);
    }

    public async Task<RemoteDriverDto?> CreateAsync(AddRemoteDriverRequest request, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAsync<AddRemoteDriverRequest, RemoteDriverDto>(BaseRoute, request, cancellationToken);
    }

    public async Task UpdateAsync(UpdateRemoteDriverRequest request, CancellationToken cancellationToken = default)
    {
        await _httpClient.PutAsync(BaseRoute, request, cancellationToken);
    }

    public async Task DeleteAsync(Guid driverId, CancellationToken cancellationToken = default)
    {
        await _httpClient.DeleteAsync($"{BaseRoute}/{driverId}", cancellationToken);
    }
}
