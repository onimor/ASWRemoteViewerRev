using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.RemoteCar;
using ASW.Shared.Requests.RemoteCar;

namespace ASW.RemoteViewing.Client.Clients;

public class RemoteCarClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;
    private const string BaseRoute = "/api/v1/ReferenceBooks/Cars";
     
    public async Task<List<RemoteCarDto>?> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetAsync<List<RemoteCarDto>>(BaseRoute, cancellationToken);
    }

    public async Task<RemoteCarDto?> GetByIdAsync(Guid carId, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetAsync<RemoteCarDto>($"{BaseRoute}/{carId}", cancellationToken);
    }

    public async Task<RemoteCarDto?> GetByNumberAsync(string number, CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetAsync<RemoteCarDto>($"{BaseRoute}?number={Uri.EscapeDataString(number)}", cancellationToken);
    }
        
    public async Task<RemoteCarDto?> CreateAsync(AddRemoteCarRequest request, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAsync<AddRemoteCarRequest, RemoteCarDto>(BaseRoute, request, cancellationToken);
    }

    public async Task UpdateAsync(UpdateRemoteCarRequest request, CancellationToken cancellationToken = default)
    {
        await _httpClient.PutAsync(BaseRoute, request, cancellationToken);
    }

    public async Task DeleteAsync(Guid carId, CancellationToken cancellationToken = default)
    {
        await _httpClient.DeleteAsync($"{BaseRoute}/{carId}", cancellationToken);
    }
}
