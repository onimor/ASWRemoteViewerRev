using ASW.RemoteViewing.Shared.Dto.RemoteGood;
using ASW.Shared.Requests.RemoteGood;

namespace ASW.RemoteViewing.Features.RemoteGood.Services;

public interface IRemoteGoodService
{
    Task<AddRemoteGoodRequest> CreateAsync(AddRemoteGoodRequest addRemoteGoodRequest); 
    Task<RemoteGoodDto?> GetByIdAsync(Guid? goodId);
    Task<List<RemoteGoodDto>> GetAllAsync(); 
    Task UpdateAsync(UpdateRemoteGoodRequest updateRemoteGoodRequest); 
    Task DeleteAsync(Guid goodId);
}
