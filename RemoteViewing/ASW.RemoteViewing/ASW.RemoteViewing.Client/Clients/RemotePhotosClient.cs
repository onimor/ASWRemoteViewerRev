using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.RemotePhoto;
using ASW.Shared.Requests.RemoteEmptyWeighingPhoto;
using ASW.Shared.Requests.RemotePhotoBrutto;
using ASW.Shared.Requests.RemotePhotoTara;
using System.Net.Http;

namespace ASW.RemoteViewing.Client.Clients;

public class RemotePhotosClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;
    private const string BaseRoute = "/api/v1/Remote/Posts";
     

    public async Task AddTaraPhotoAsync(AddRemotePhotoTaraRequest request, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseRoute}/Weighings/{request.WeighingId}/Photos/Tara";
        await _httpClient.PostAsync(url, request, cancellationToken);
    }

    public async Task<List<RemotePhotoTaraDto>?> GetTaraPhotosAsync(Guid weighingId, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseRoute}/Weighings/{weighingId}/Photos/Tara";
        return await _httpClient.GetAsync<List<RemotePhotoTaraDto>>(url, cancellationToken);
    }

    public async Task DeleteTaraPhotosAsync(Guid weighingId, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseRoute}/Weighings/{weighingId}/Photos/Tara";
        await _httpClient.DeleteAsync(url, cancellationToken);
    }

    public async Task AddBruttoPhotoAsync(AddRemotePhotoBruttoRequest request, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseRoute}/Weighings/{request.WeighingId}/Photos/Brutto";
        await _httpClient.PostAsync(url, request, cancellationToken);
    }

    public async Task<List<RemotePhotoBruttoDto>?> GetBruttoPhotosAsync(Guid weighingId, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseRoute}/Weighings/{weighingId}/Photos/Brutto";
        return await _httpClient.GetAsync<List<RemotePhotoBruttoDto>>(url, cancellationToken);
    }

    public async Task DeleteBruttoPhotosAsync(Guid weighingId, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseRoute}/Weighings/{weighingId}/Photos/Brutto";
        await _httpClient.DeleteAsync(url, cancellationToken);
    }

    public async Task AddEmptyWeighingPhotoAsync(AddRemoteEmptyWeighingPhotoRequest request, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseRoute}/EmptyWeighings/{request.EmptyWeighingId}/Photos";
        await _httpClient.PostAsync(url, request, cancellationToken);
    }

    public async Task<List<RemoteEmptyWeighingPhotoDto>?> GetEmptyWeighingPhotosAsync(Guid emptyWeighingId, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseRoute}/EmptyWeighings/{emptyWeighingId}/Photos";
        return await _httpClient.GetAsync<List<RemoteEmptyWeighingPhotoDto>>(url, cancellationToken);
    }

    public async Task DeleteEmptyWeighingPhotosAsync(Guid emptyWeighingId, CancellationToken cancellationToken = default)
    {
        var url = $"{BaseRoute}/EmptyWeighings/{emptyWeighingId}/Photos";
        await _httpClient.DeleteAsync(url, cancellationToken);
    }
}
