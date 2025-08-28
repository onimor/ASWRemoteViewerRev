using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.IntegrationUser;
using ASW.RemoteViewing.Shared.Requests.IntegrationUser;
using ASW.RemoteViewing.Shared.Responses.Authentication;

namespace ASW.RemoteViewing.Client.Clients;

public class IntegrationUserClient(SafeHttpClient httpClient)
{
    private readonly SafeHttpClient _httpClient = httpClient;
    private const string BaseRoute = "/api/v1/IntegrationUsers";

    /// <summary> Получить всех клиентов </summary>
    public async Task<List<IntegrationUserDto>?> GetAllAsync()
    {
        return await _httpClient.GetAsync<List<IntegrationUserDto>>(BaseRoute);
    }

    /// <summary> Получить клиента по ID </summary>
    public async Task<IntegrationUserDto?> GetByIdAsync(Guid userId)
    {
        return await _httpClient.GetAsync<IntegrationUserDto>($"{BaseRoute}/{userId}");
    }

    /// <summary> Создать нового клиента </summary>
    public async Task<LoginResponse?> CreateAsync(CreateIntegrationUserRequest request)
    {
        var result = await _httpClient.PostAsync<CreateIntegrationUserRequest, LoginResponse?>(BaseRoute, request);
        return result;
    }

    /// <summary> Обновить клиента по ID </summary>
    public async Task UpdateAsync(Guid userId, UpdateIntegrationUserRequest request)
    {
        await _httpClient.PutAsync($"{BaseRoute}/{userId}", request);
    }
    /// <summary> Перевыпустить ключ по ID </summary>
    public async Task<LoginResponse?> UpdateKeyAsync(Guid userId)
    {
        var result = await _httpClient.PostAsync<LoginResponse?>($"{BaseRoute}/{userId}/Key");
        return result;
    }

    /// <summary> Удалить клиента по ID </summary>
    public async Task DeleteAsync(Guid userId)
    {
        await _httpClient.DeleteAsync($"{BaseRoute}/{userId}");
    }
}