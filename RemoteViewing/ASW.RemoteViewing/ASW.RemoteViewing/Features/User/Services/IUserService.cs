using ASW.RemoteViewing.Features.User.Entities;
using ASW.RemoteViewing.Shared.Dto.User;
using ASW.RemoteViewing.Shared.Requests.User;

namespace ASW.RemoteViewing.Features.User.Services;

public interface IUserService
{
    Task Create(CreateUserRequest newUser);

    Task<UserEF?> GetById(Guid userId); 
    Task<List<UserDto>> GetAll();

    Task Update(Guid userId, UpdateUserRequest updateUser);

    Task Delete(UserEF user);
}
