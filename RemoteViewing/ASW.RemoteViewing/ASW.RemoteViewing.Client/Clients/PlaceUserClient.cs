using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.PlaceUser;
using ASW.RemoteViewing.Shared.Requests.PlaceUser;
using ASW.RemoteViewing.Shared.Responses.Authentication;

namespace ASW.RemoteViewing.Client.Clients;

public class PlaceUserClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;
    private const string BaseRoute = "/api/v1/PlaceUsers";

    /// <summary> Получить всех клиентов </summary>
    public async Task<List<PlaceUserDto>?> GetAllAsync()
    {
        return await _httpClient.GetAsync<List<PlaceUserDto>>(BaseRoute);
    }

    /// <summary> Получить клиента по ID </summary>
    public async Task<PlaceUserDto?> GetByIdAsync(Guid userId)
    {
        return await _httpClient.GetAsync<PlaceUserDto>($"{BaseRoute}/{userId}");
    }

    /// <summary> Создать нового клиента </summary>
    public async Task<LoginResponse?> CreateAsync(CreatePlaceUserRequest request)
    {
        return await _httpClient.PostAsync<CreatePlaceUserRequest,LoginResponse>(BaseRoute, request);
    }

    /// <summary> Обновить клиента по ID </summary>
    public async Task UpdateAsync(Guid userId, UpdatePlaceUserRequest request)
    {
        await _httpClient.PutAsync($"{BaseRoute}/{userId}", request);
    }
    /// <summary> Перевыпустить токен клиента по ID </summary>
    public async Task<LoginResponse?> UpdateKeyAsync(Guid userId)
    {
        return await _httpClient.PostAsync<LoginResponse>($"{BaseRoute}/{userId}/Key");
    }
    /// <summary> Удалить клиента по ID </summary>
    public async Task DeleteAsync(Guid userId)
    {
        await _httpClient.DeleteAsync($"{BaseRoute}/{userId}");
    }
}