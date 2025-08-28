using ASW.RemoteViewing.Shared.Dto.IntegrationUser;
using ASW.RemoteViewing.Shared.Requests.IntegrationUser;
using ASW.RemoteViewing.Shared.Responses.Authentication;

namespace ASW.RemoteViewing.Features.IntegrationUser.Services;

public interface IIntegrationUserService
{
    Task<LoginResponse> CreateAsync(CreateIntegrationUserRequest addApiKeyClientRequest);  
    Task<List<IntegrationUserDto>> GetAllAsync();
    Task<IntegrationUserDto?> GetByIdAsync(Guid integrationUserId);
    Task<IntegrationUserDto?> GetByKey(string key);

    Task UpdateAsync(Guid integrationUserId, UpdateIntegrationUserRequest updateIntegrationUserRequest);
    Task<LoginResponse>UpdateKeyAsync(Guid integrationUserId);

    Task DeleteAsync(Guid integrationUserId);
}
