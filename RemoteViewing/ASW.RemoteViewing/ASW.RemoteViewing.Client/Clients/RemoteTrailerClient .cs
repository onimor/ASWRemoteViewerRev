using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.RemoteTrailer;
using ASW.Shared.Requests.RemoteTrailer;

namespace ASW.RemoteViewing.Client.Clients;

public class RemoteTrailerClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;
    private const string BaseRoute = "/api/v1/ReferenceBooks/Trailers"; 

    public async Task<List<RemoteTrailerDto>?> GetAllAsync()
    {
        return await _httpClient.GetAsync<List<RemoteTrailerDto>>(BaseRoute);
    }

    public async Task<RemoteTrailerDto?> GetByIdAsync(Guid trailerId)
    {
        return await _httpClient.GetAsync<RemoteTrailerDto>($"{BaseRoute}/{trailerId}");
    }

    public async Task CreateAsync(AddRemoteTrailerRequest request)
    {
        await _httpClient.PostAsync(BaseRoute, request);
    }

    public async Task UpdateAsync(UpdateRemoteTrailerRequest request)
    {
        await _httpClient.PutAsync(BaseRoute, request);
    }

    public async Task DeleteAsync(Guid trailerId)
    {
        await _httpClient.DeleteAsync($"{BaseRoute}/{trailerId}");
    }
}
