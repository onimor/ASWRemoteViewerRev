using ASW.RemoteViewing.Shared.Dto.RemoteDriver;
using ASW.Shared.Requests.RemoteDriver;

namespace ASW.RemoteViewing.Features.RemoteDriver.Services;

public interface IRemoteDriverService
{
    Task<AddRemoteDriverRequest> CreateAsync(AddRemoteDriverRequest addRemoteDriverRequest); 
    Task<RemoteDriverDto?> GetByIdAsync(Guid driverId);
    Task<List<RemoteDriverDto>> GetAllAsync(); 
    Task UpdateAsync(UpdateRemoteDriverRequest updateRemoteDriverRequest); 
    Task DeleteAsync(Guid driverId);
}
