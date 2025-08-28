using ASW.RemoteViewing.Shared.Dto.RemoteWeighing;
using ASW.RemoteViewing.Shared.Requests.RemoteWeighing;
using ASW.Shared.Requests.RemoteWeighing;

namespace ASW.RemoteViewing.Features.RemoteWeighing.Services;

public interface IRemoteWeighingService
{
    Task<AddRemoteWeighingRequest> CreateAsync(AddRemoteWeighingRequest addRemoteWeighingRequest);
    Task<RemoteWeighingDto?> GetByIdAsync(Guid weighingId);
    Task<List<RemoteWeighingDto>> GetAllAsync();
    Task<List<RemoteWeighingDto>> GetLastOneWeighingAsync(Guid postId);
    Task<List<RemoteWeighingDto>> GetByDateAsync(RemoteWeightByDateRequest remoteWeightByDateRequest);
    Task<List<RemoteWeighingDto>> GetByPostAndDateAsync(RemoteWeightByDateRequest remoteWeightByDateRequest, Guid postId);
    Task UpdateAsync(UpdateRemoteWeighingRequest updateRemoteWeighingRequest);
    Task DeleteAsync(Guid remoteWeighingId);
}
