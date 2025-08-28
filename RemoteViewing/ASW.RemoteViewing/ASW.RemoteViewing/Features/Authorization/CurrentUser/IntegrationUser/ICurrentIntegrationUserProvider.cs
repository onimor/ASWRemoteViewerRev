using ASW.RemoteViewing.Shared.Dto.IntegrationUser;

namespace ASW.RemoteViewing.Features.Authorization.CurrentUser.IntegrationUser;

public interface ICurrentIntegrationUserProvider
{
    Task<IntegrationUserDto> GetCurrentUserAsync();
}
