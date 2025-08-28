using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Shared.Dto.User;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace ASW.RemoteViewing.Client.Services.Token;

public class TokenProvider : ITokenProvider
{
    private readonly ILocalStorageService _localStorage;
    private readonly ISessionStorageService _sessionStorage;
    private readonly TokenValidationHttpClient _wrapper;
    private const string TokenKey = "authToken";
    public TokenProvider(
        ILocalStorageService localStorage, 
        ISessionStorageService sessionStorage, 
        TokenValidationHttpClient wrapper)
    {
        _localStorage = localStorage;
        _sessionStorage = sessionStorage;
        _wrapper = wrapper;
    }
    public async Task<UserInfoDto?> ValidateTokenAsync(string token)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "/api/v1/Auth/ValidateToken");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            using var response = await _wrapper.Client.SendAsync(request);
            if(response.IsSuccessStatusCode == false)
            {
                return default; 
            }
            return await response.Content.ReadFromJsonAsync<UserInfoDto?>();
        }
        catch
        {
            return default;
        }
    }
   
    public async Task<string?> GetTokenAsync()
    {
        var sessionToken = await _sessionStorage.GetItemAsync<string>(TokenKey);
      
        return string.IsNullOrEmpty(sessionToken) 
            ? await _localStorage.GetItemAsync<string>(TokenKey)
            : sessionToken;
    }

    public async Task RemoveTokenAsync()
    {
        await _sessionStorage.RemoveItemAsync(TokenKey);
        await _localStorage.RemoveItemAsync(TokenKey);
    }

    public async Task SetTokenAsync(string token, bool isRememberMe)
    { 
        if(isRememberMe)
            await _localStorage.SetItemAsync(TokenKey, token);
        else
            await _sessionStorage.SetItemAsync(TokenKey, token);
    }
}