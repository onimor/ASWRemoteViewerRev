using ASW.RemoteViewing.Shared.Dto.RemoteTrailer;
using ASW.Shared.Requests.RemoteTrailer;

namespace ASW.RemoteViewing.Features.RemoteTrailer.Services;

public interface IRemoteTrailerService
{
    Task<AddRemoteTrailerRequest> CreateAsync(AddRemoteTrailerRequest newTrailer); 
    Task<RemoteTrailerDto?> GetByIdAsync(Guid trailerId);
    Task<List<RemoteTrailerDto>> GetAllAsync(); 
    Task UpdateAsync(UpdateRemoteTrailerRequest trailerDto); 
    Task DeleteAsync(Guid trailerId); 
}
