using ASW.RemoteViewing.Shared.Dto.RemoteAxes;
using ASW.Shared.Requests.RemoteAxesVel;

namespace ASW.RemoteViewing.Features.RemoteAxesVel.Services;

public interface IRemoteAxesVelService
{
    Task CreateAsync(AddRemoteAxesVelRequest addRemoteAxesVelRequest);
    Task DeleteAsync(Guid weighingId);
    Task<List<RemoteAxesVelDto>?> GetByWeighingAsync(Guid weighingId);
}
