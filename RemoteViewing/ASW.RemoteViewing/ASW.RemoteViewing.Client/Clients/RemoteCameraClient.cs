using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.RemoteCamera;
using ASW.Shared.Requests.RemoteCamera;

namespace ASW.RemoteViewing.Client.Clients;

public class RemoteCameraClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;
    private const string BaseRoute = "/api/v1/Remote/Posts/Cameras";
     
    public async Task<List<RemoteCameraDto>?> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _httpClient.GetAsync<List<RemoteCameraDto>>(BaseRoute, cancellationToken);
    }

    public async Task<List<RemoteCameraDto>?> GetAllByPostAsync(Guid postId, CancellationToken cancellationToken = default)
    {
        var url = $"/api/v1/Remote/Posts/{postId}/Cameras";
        return await _httpClient.GetAsync<List<RemoteCameraDto>>(url, cancellationToken);
    }

    public async Task<RemoteCameraDto?> GetByIdAsync(Guid cameraId, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseRoute}/{cameraId}";
        return await _httpClient.GetAsync<RemoteCameraDto>(url, cancellationToken);
    }

    public async Task<RemoteCameraDto?> CreateAsync(AddRemoteCameraRequest request, CancellationToken cancellationToken = default)
    {
        return await _httpClient.PostAsync<AddRemoteCameraRequest, RemoteCameraDto>(BaseRoute, request, cancellationToken);
    }

    public async Task UpdateAsync(UpdateRemoteCameraRequest request, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseRoute}/{request.Id}";
        await _httpClient.PutAsync(url, request, cancellationToken);
    }

    public async Task DeleteAsync(Guid cameraId, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseRoute}/{cameraId}";
        await _httpClient.DeleteAsync(url, cancellationToken);
    }

    public async Task DeleteByPostAsync(Guid postId, CancellationToken cancellationToken = default)
    {
        var url = $"/api/v1/Remote/Posts/{postId}/Cameras";
        await _httpClient.DeleteAsync(url, cancellationToken);
    }
}