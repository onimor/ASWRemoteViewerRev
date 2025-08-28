using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.RemotePostUsers;
using ASW.Shared.Requests.RemotePostUsers;

namespace ASW.RemoteViewing.Client.Clients;

public class RemotePostUsersClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;
    private const string BaseRoute = "/api/v1/Remote/Posts";

    public Task<List<RemotePostUserDto>?> GetByPostAsync(Guid postId, CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<List<RemotePostUserDto>>($"{BaseRoute}/{postId}/Users", cancellationToken);
    }

    public Task CreateAsync(AddRemotePostUsersRequest request, CancellationToken cancellationToken = default)
    {
        return _httpClient.PostAsync($"{BaseRoute}/{request.PostId}/Users", request, cancellationToken);
    }

    public Task DeleteByPostAsync(Guid postId, CancellationToken cancellationToken = default)
    {
        return _httpClient.DeleteAsync($"{BaseRoute}/{postId}/Users", cancellationToken);
    }

    public Task<List<RemotePostUserDto>?> GetByUserAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        return _httpClient.GetAsync<List<RemotePostUserDto>>($"/api/v1/Remote/Users/{userId}/Posts", cancellationToken);
    }
}
