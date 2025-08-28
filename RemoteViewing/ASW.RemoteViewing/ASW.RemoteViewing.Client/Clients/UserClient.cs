using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.User;
using ASW.RemoteViewing.Shared.Requests.User;

namespace ASW.RemoteViewing.Client.Clients;

public class UserClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;
    private const string BaseRoute = "/api/v1/Users";

    /// <summary> Получить всех пользователей </summary>
    public async Task<List<UserDto>?> GetAllAsync()
    {
        return await _httpClient.GetAsync<List<UserDto>>(BaseRoute);
    }

    /// <summary> Получить пользователя по ID </summary>
    public async Task<UserDto?> GetByIdAsync(Guid userId)
    {
        return await _httpClient.GetAsync<UserDto>($"{BaseRoute}/{userId}");
    }

    /// <summary> Создать нового пользователя </summary>
    public async Task CreateAsync(CreateUserRequest request)
    {
        await _httpClient.PostAsync(BaseRoute, request);
    }

    /// <summary> Обновить пользователя по ID </summary>
    public async Task UpdateAsync(Guid userId, UpdateUserRequest request)
    {
        await _httpClient.PutAsync($"{BaseRoute}/{userId}", request);
    }

    /// <summary> Удалить пользователя по ID </summary>
    public async Task DeleteAsync(Guid userId)
    {
        await _httpClient.DeleteAsync($"{BaseRoute}/{userId}");
    }
}