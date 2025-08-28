using ASW.RemoteViewing.Shared.Dto.RemoteAxes;
using ASW.Shared.Requests.RemoteAxesWeight;

namespace ASW.RemoteViewing.Features.RemoteAxesWeight.Services;

public interface IRemoteAxesWeightService
{
    Task CreateAsync(AddRemoteAxesWeightRequest addRemoteAxesWeightRequests);
    Task DeleteAsync(Guid weighingId);
    Task<List<RemoteAxesWeightDto>?> GetByWeighingAsync(Guid weighingId);
}
