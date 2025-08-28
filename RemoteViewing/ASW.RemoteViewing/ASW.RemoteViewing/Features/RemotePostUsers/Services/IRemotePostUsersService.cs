using ASW.RemoteViewing.Shared.Dto.RemotePostUsers;
using ASW.Shared.Requests.RemotePostUsers;

namespace ASW.RemoteViewing.Features.RemotePostUsers.Services;

public interface IRemotePostUsersService
{
    Task CreateAsync(AddRemotePostUsersRequest addPostUsersRequest);

    Task<List<RemotePostUserDto>?> GetByPostAsync(Guid postId);
    Task<List<RemotePostUserDto>?> GetByUserAsync(Guid userId);
    Task DeleteByPostAsync(Guid postId); 
}
