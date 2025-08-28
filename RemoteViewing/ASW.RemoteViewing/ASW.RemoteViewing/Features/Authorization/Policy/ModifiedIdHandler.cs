using ASW.RemoteViewing.Features.PlaceUser.Services;
using ASW.RemoteViewing.Features.User.Services;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace ASW.RemoteViewing.Features.Authorization.Policy;

public class ModifiedIdHandler : AuthorizationHandler<ModifiedIdRequirement>
{
    private readonly IUserService _userService;
    private readonly IPlaceUserService _placeUserService;

    public ModifiedIdHandler(IUserService userService, IPlaceUserService placeUserService)
    {
        _userService = userService;
        _placeUserService = placeUserService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ModifiedIdRequirement requirement)
    {
        var claims = context.User.Claims.ToList();
        var modifiedId = claims.FirstOrDefault(x => x.Type == "modified_id")?.Value;
        var scheme = claims.FirstOrDefault(x => x.Type == "auth_scheme")?.Value;
        var userIdStr = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrWhiteSpace(modifiedId))
            return; 

        if (!Guid.TryParse(userIdStr, out var userId))
            return;

        switch (scheme)
        {
            case AuthSchemes.UserJwt:
                var user = await _userService.GetById(userId);
                if (user?.ModifiedId.ToString() == modifiedId)
                    context.Succeed(requirement);
                break;

            case AuthSchemes.PlaceUserJwt:
                var placeUser = await _placeUserService.GetByIdAsync(userId);
                if (placeUser?.ModifiedId.ToString() == modifiedId)
                    context.Succeed(requirement);
                break;

            case AuthSchemes.IntegrationUser:
                context.Succeed(requirement); // Пропускаем проверку
                break;

            default:
                //Отказать
                context.Fail();
                break;
        }
    }
}