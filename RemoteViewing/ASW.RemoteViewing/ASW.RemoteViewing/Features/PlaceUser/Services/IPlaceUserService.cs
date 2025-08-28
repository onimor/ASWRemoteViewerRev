using ASW.RemoteViewing.Shared.Dto.PlaceUser;
using ASW.RemoteViewing.Shared.Requests.PlaceUser;
using ASW.RemoteViewing.Shared.Responses.Authentication;

namespace ASW.RemoteViewing.Features.PlaceUser.Services;

public interface IPlaceUserService
{
    Task<LoginResponse> CreateAsync(CreatePlaceUserRequest addPlaceUserRequest);
    Task<List<PlaceUserDto>> GetAllAsync();
    Task<PlaceUserDto?> GetByIdAsync(Guid placeUserId);

    Task UpdateAsync(Guid placeUserId, UpdatePlaceUserRequest updatePlaceUserRequest);
    Task<LoginResponse> UpdateJWT(Guid integrationUserId);

    Task DeleteAsync(Guid placeUserId);
}
