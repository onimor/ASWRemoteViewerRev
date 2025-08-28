using ASW.RemoteViewing.Shared.Dto.RemoteAxes;
using ASW.Shared.Requests.RemoteAxesDist;

namespace ASW.RemoteViewing.Features.RemoteAxesDist.Services;

public interface IRemoteAxesDistService
{
    public Task<AddRemoteAxesDistRequest> CreateAsync(AddRemoteAxesDistRequest addRemoteAxesDistRequests);
    Task DeleteAsync(Guid weighingId);
    public Task<List<RemoteAxesDistDto>?> GetByWeighingAsync(Guid weighingId);
}
