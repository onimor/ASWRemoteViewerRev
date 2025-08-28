using ASW.RemoteViewing.Features.PlaceUser.Entities;
using ASW.RemoteViewing.Shared.Dto.PlaceUser;
using ASW.RemoteViewing.Shared.Requests.PlaceUser;

namespace ASW.RemoteViewing.Features.PlaceUser.Mapping;

public static class PlaceUserMapping
{
    public static PlaceUserDto ToDto(this PlaceUserEF entity)
    {
        return new PlaceUserDto
        {
            Id = entity.Id,
            Name = entity.Name,
            ModifiedId = entity.ModifiedId
        };
    }

    public static PlaceUserEF ToEntity(this CreatePlaceUserRequest request)
    {
        return new PlaceUserEF
        { 
            Name = request.Name
        };
    }
}
