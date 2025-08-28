using ASW.RemoteViewing.Shared.Dto.PlaceUser;

namespace ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;

public interface ICurrentPlaceUserProvider
{
    Task<PlaceUserDto> GetCurrentUserAsync();
}
