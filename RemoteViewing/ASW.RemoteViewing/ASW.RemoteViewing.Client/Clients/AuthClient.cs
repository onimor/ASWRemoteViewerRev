using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.User;
using ASW.RemoteViewing.Shared.Requests;
using ASW.RemoteViewing.Shared.Requests.IntegrationUser;
using ASW.RemoteViewing.Shared.Requests.PlaceUser;
using ASW.RemoteViewing.Shared.Responses.Authentication;

namespace ASW.RemoteViewing.Client.Clients;

public class AuthClient
{
    private readonly SafeHttpClient _httpClient;

    public AuthClient(SafeHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<LoginResponse?> LoginAsync(string username, string password)
    {
        var request = new LoginRequest
        {
            Login = username,
            Password = password
        };

        return await _httpClient.PostAsync<LoginRequest, LoginResponse>("/api/v1/Auth/Login", request);
    }
   
    public async Task<LoginResponse?> LoginIntegrationAsync(string name)
    {
        var request = new CreateIntegrationUserRequest
        {
            Name = name
        };

        return await _httpClient.PostAsync<CreateIntegrationUserRequest, LoginResponse>("/api/v1/Auth/LoginIntegration", request);
    }

    public async Task<LoginResponse?> LoginPlaceAsync(string name)
    {
        var request = new CreatePlaceUserRequest
        {
            Name = name
        };

        return await _httpClient.PostAsync<CreatePlaceUserRequest, LoginResponse>("/api/v1/Auth/LoginPlace", request);
    }
}
