using ASW.RemoteViewing.Shared.Dto.User;

namespace ASW.RemoteViewing.Features.Authorization.CurrentUser.Default;

public interface ICurrentUserProvider
{
    Task<UserInfoDto> GetCurrentUserAsync();
}
