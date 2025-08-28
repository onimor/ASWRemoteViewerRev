
using ASW.RemoteViewing.Shared.Requests;
using ASW.RemoteViewing.Shared.Responses.Authentication;
using ASW.RemoteViewing.Shared.Dto.User;

namespace ASW.RemoteViewing.Features.Authorization.Services;

public interface IAuthorizationService
{
    Task<LoginResponse> LogIn(LoginRequest userLogin);  
    Task<UserInfoDto?> ValidateTokenState(string token); 
}
