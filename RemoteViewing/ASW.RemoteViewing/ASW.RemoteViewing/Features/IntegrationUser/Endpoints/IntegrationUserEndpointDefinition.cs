using ASW.RemoteViewing.Features.IntegrationUser.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.IntegrationUser;
using ASW.RemoteViewing.Shared.Dto.User;
using ASW.RemoteViewing.Shared.Requests.IntegrationUser;
using ASW.RemoteViewing.Shared.Security;
using System.Security.Claims;

namespace ASW.RemoteViewing.Features.User.Endpoints;

public class IntegrationUserEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/v1/IntegrationUsers")
            .WithTags("IntegrationUsers")
            .RequireAuthorization();

        group.MapGet("/", GetAll)
            .WithName("GetAllIntegrationUsers")
            .WithSummary("Получить всех интеграционных пользователей")
            .Produces<List<UserDto>>(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.IntegrationUser.CanView);

        group.MapGet("/{integrationUserId:guid}", GetById)
            .WithName("GetIntegrationUserById")
            .WithSummary("Получить интеграционного пользователя по Id")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.IntegrationUser.CanView);

        group.MapPost("/", Create)
            .WithName("CreateIntegrationUser")
            .WithSummary("Создание нового интеграционного пользователя")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.IntegrationUser.CanAdd);

        group.MapPut("/{integrationUserId:guid}", Update)
            .WithName("UpdateIntegrationUser")
            .WithSummary("Изменить интеграционного пользователя")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.IntegrationUser.CanEdit);

        group.MapPost("/{integrationUserId:guid}/Key", UpdateKey)
            .WithName("UpdateIntegrationUserKey")
            .WithSummary("Перевыпустить ключ интеграционного пользователя")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.IntegrationUser.CanEdit);

        group.MapDelete("/{integrationUserId:guid}", Delete)
            .WithName("DeleteIntegrationUser")
            .WithSummary("Удалить интеграционного пользователя")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.IntegrationUser.CanDelete);
    }

    internal async Task<IResult> GetAll(IIntegrationUserService service, ClaimsPrincipal user)
    {
        var users = await service.GetAllAsync();
        return Results.Ok(users);
    }

    internal async Task<IResult> GetById(IIntegrationUserService service, Guid integrationUserId)
    {
        var user = await service.GetByIdAsync(integrationUserId);
        return user is null ? Results.NotFound() : Results.Ok(new IntegrationUserDto
        {
            Id = user.Id,
            Name = user.Name,
            Role = user.Role,
            KeyPrefix = user.KeyPrefix
        });
    }

    internal async Task<IResult> Create(IIntegrationUserService service, CreateIntegrationUserRequest request)
    {
        var result = await service.CreateAsync(request);
        return Results.Ok(result);
    }

    internal async Task<IResult> Update(IIntegrationUserService service, Guid integrationUserId, UpdateIntegrationUserRequest request)
    {
        await service.UpdateAsync(integrationUserId, request);
        return Results.Ok();
    }
    internal async Task<IResult> UpdateKey(IIntegrationUserService service, Guid integrationUserId)
    {
        var result = await service.UpdateKeyAsync(integrationUserId);
        return Results.Ok(result);
    }

    internal async Task<IResult> Delete(IIntegrationUserService service, Guid integrationUserId)
    {
        var user = await service.GetByIdAsync(integrationUserId);
        if (user is null)
            return Results.NotFound();

        await service.DeleteAsync(integrationUserId);
        return Results.Ok();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IIntegrationUserService, IntegrationUserService>();
    }
}