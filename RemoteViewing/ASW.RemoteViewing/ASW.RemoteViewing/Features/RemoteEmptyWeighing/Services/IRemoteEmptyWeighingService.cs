using ASW.RemoteViewing.Shared.Dto.RemoteEmptyWeighing;
using ASW.RemoteViewing.Shared.Requests.RemoteWeighing;
using ASW.Shared.Requests.RemoteEmptyWeighing;

namespace ASW.RemoteViewing.Features.RemoteEmptyWeighing.Services;

public interface IRemoteEmptyWeighingService
{
    Task<AddRemoteEmptyWeighingRequest> CreateAsync(AddRemoteEmptyWeighingRequest addRemoteEmptyWeighingRequest);

    Task<RemoteEmptyWeighingDto?> GetByIdAsync(Guid emptyWeighingId);
    Task<List<RemoteEmptyWeighingDto>> GetAllAsync();
    Task<List<RemoteEmptyWeighingDto>> GetByDateAsync(RemoteWeightByDateRequest weightByDateRequest);
    Task<List<RemoteEmptyWeighingDto>> GetByPostAndDateAsync(RemoteWeightByDateRequest weightByDateRequest, Guid postId);

    Task UpdateAsync(UpdateRemoteEmptyWeighingRequest updateRemoteEmptyWeighingRequest);

    Task DeleteAsync(Guid weighingId);
}
