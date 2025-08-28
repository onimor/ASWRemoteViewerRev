using ASW.RemoteViewing.Shared.Dto.RemoteCounterparty;
using ASW.Shared.Requests.RemoteCounterparty;

namespace ASW.RemoteViewing.Features.RemoteCounterparty.Services;

public interface IRemoteCounterpartyService
{
    Task<AddRemoteCounterpartyRequest> CreateAsync(AddRemoteCounterpartyRequest addRemoteCounterpartyRequest);
    Task<RemoteCounterpartyDto?> GetByIdAsync(Guid counterpartyId); 
    Task<List<RemoteCounterpartyDto>> GetAllAsync(); 
    Task UpdateAsync(UpdateRemoteCounterpartyRequest updateRemoteCounterpartyRequest); 
    Task DeleteAsync(Guid counterpartyId);
}
