using ASW.RemoteViewing.Shared.Dto.User;

namespace ASW.RemoteViewing.Client.Services.Token;

public interface ITokenProvider
{
    Task<string?> GetTokenAsync();
    Task SetTokenAsync(string token, bool isRememberMe);
    Task RemoveTokenAsync();
    Task<UserInfoDto?> ValidateTokenAsync(string token);
}
