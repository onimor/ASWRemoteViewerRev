using ASW.RemoteViewing.Features.User.Entities;
using ASW.RemoteViewing.Shared.Dto.User;
using ASW.RemoteViewing.Shared.Requests.User;

namespace ASW.RemoteViewing.Features.User.Mapping;

public static class UserMapping
{
    public static UserDto ToDto(this UserEF entity)
    {
        return new UserDto
        {
            Id = entity.Id,
            FIO = entity.ReductionFIO,
            Role = entity.Role,
            Login = entity.Login,
        };
    }

    public static UserEF ToEntity(this CreateUserRequest request)
    {
        return new UserEF
        {
            Login = request.Login,
            FullFIO = request.FullFIO,
            ReductionFIO = request.ReductionFIO,
        };
    }
}
