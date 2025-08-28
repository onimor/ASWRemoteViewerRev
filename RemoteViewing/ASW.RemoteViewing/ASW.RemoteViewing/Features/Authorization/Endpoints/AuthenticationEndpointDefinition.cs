using ASW.RemoteViewing.Features.Authorization.Services;
using ASW.RemoteViewing.Features.IntegrationUser.Services;
using ASW.RemoteViewing.Features.PlaceUser.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Constants;
using ASW.RemoteViewing.Shared.Dto.User;
using ASW.RemoteViewing.Shared.Requests;
using ASW.RemoteViewing.Shared.Requests.IntegrationUser;
using ASW.RemoteViewing.Shared.Requests.PlaceUser;

namespace ASW.RemoteViewing.Features.Authorization.Endpoints;

public class AuthenticationEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/v1/Auth")
            .WithTags("Authentication");

        group.MapPost("/Login", Login)
            .WithName("LoginUser")
            .WithSummary("Аутентификация пользователя")
            .WithDescription("Осуществляет вход пользователя по логину и паролю, возвращает JWT токен при успешной аутентификации.")
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .AllowAnonymous()
            .ExcludeFromDescription();

        group.MapPost("/LoginIntegration", LoginIntegration)
            .WithName("LoginIntegrationUser")
            .WithSummary("Аутентификация Integration-пользователя")
            .WithDescription("Вход с использованием API-ключа для интеграции. Возвращает токен.")
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .AllowAnonymous()
            .ExcludeFromDescription();

        group.MapPost("/LoginPlace", LoginPlace)
            .WithName("LoginPlaceUser")
            .WithSummary("Аутентификация пользователя поста")
            .WithDescription("Вход через физический пост с привязкой к устройству. Возвращает токен.")
            .Produces<string>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .AllowAnonymous()
            .ExcludeFromDescription();

        group.MapGet("/ValidateToken", ValidateToken)
           .WithName("ValidateToken")
           .WithSummary("Проверка актуальности токена")
           .Produces<UserInfoDto>(StatusCodes.Status200OK)
           .Produces(StatusCodes.Status401Unauthorized)
           .ExcludeFromDescription()
           .RequireAuthorization(Shared.Security.Policies.PlaceUser.CanView);
    }

    internal async Task<IResult> Login(IAuthorizationService service, LoginRequest request)
    {
        var token = await service.LogIn(request);
        return token is null ? Results.NotFound() : Results.Ok(token);
    }

    internal async Task<IResult> LoginIntegration(IIntegrationUserService service, CreateIntegrationUserRequest request)
    {
        var token = await service.CreateAsync(request);
        return token is null ? Results.NotFound() : Results.Ok(token);
    }

    internal async Task<IResult> LoginPlace(IPlaceUserService service, CreatePlaceUserRequest request)
    {
        var token = await service.CreateAsync(request);
        return token is null ? Results.NotFound() : Results.Ok(token);
    }
    internal async Task<IResult> ValidateToken(IAuthorizationService service, HttpContext httpContext)
    {
        var authHeader = httpContext.Request.Headers.Authorization.ToString();
        var result = await service.ValidateTokenState(authHeader);
        return Results.Ok(result);
    }
    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IAuthorizationService, AuthorizationService>();
        services.AddScoped<IIntegrationUserService, IntegrationUserService>();
        services.AddScoped<IPlaceUserService, PlaceUserService>();
    }
}