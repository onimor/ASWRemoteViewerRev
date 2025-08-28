using ASW.RemoteViewing.Shared.Dto.RemotePhoto;
using ASW.Shared.Requests.RemoteEmptyWeighingPhoto;
using ASW.Shared.Requests.RemotePhotoBrutto;
using ASW.Shared.Requests.RemotePhotoTara;

namespace ASW.RemoteViewing.Features.RemotePhotos.Services;

public interface IRemotePhotosService
{ 
    public Task AddTaraPhotoAsync(AddRemotePhotoTaraRequest photoTara);
    public Task<List<RemotePhotoTaraDto>> GetTaraPhotoAsync(Guid weighingId);
    public Task AddBruttoPhotoAsync(AddRemotePhotoBruttoRequest photoBrutto);
    public Task<List<RemotePhotoBruttoDto>> GetBruttoPhotoAsync(Guid weighingId);
    public Task AddEmptyWeighingPhotoAsync(AddRemoteEmptyWeighingPhotoRequest emptyWeighingPhoto);
    public Task<List<RemoteEmptyWeighingPhotoDto>?> GetEmptyWeighingPhotoAsync(Guid emptyWeighingId);
    public Task DeleteTaraPhotosAsync(Guid weighingId);
    public Task DeleteBruttoPhotosAsync(Guid weighingId);
    public Task DeleteEmptyWeighingPhotosAsync(Guid emptyWeighingPhotoId);
}
