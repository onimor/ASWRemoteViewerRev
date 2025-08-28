using ASW.RemoteViewing.Features.PlaceUser.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.PlaceUser;
using ASW.RemoteViewing.Shared.Dto.User;
using ASW.RemoteViewing.Shared.Requests.PlaceUser;
using ASW.RemoteViewing.Shared.Security;
using System.Security.Claims;

namespace ASW.RemoteViewing.Features.User.Endpoints;

public class PlaceUserEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/v1/PlaceUsers")
            .WithTags("PlaceUsers")
            .RequireAuthorization();

        group.MapGet("/", GetAll)
            .WithName("GetAllPlaceUser")
            .WithSummary("Получить всех клиентских пользователей")
            .Produces<List<UserDto>>(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.PlaceUser.CanView);

        group.MapGet("/{placeUserId:guid}", GetById)
            .WithName("GetPlaceUserById")
            .WithSummary("Получить клиентского пользователя по Id")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.PlaceUser.CanView);

        group.MapPost("/", Create)
            .WithName("CreatePlaceUser")
            .WithSummary("Создание нового клиентского пользователя")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.PlaceUser.CanAdd);

        group.MapPut("/{placeUserId:guid}", Update)
            .WithName("UpdatePlaceUser")
            .WithSummary("Изменить клиентского пользователя")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.PlaceUser.CanEdit);

        group.MapPost("/{placeUserId:guid}/Key", UpdateKey)
            .WithName("UpdatePlaceUserKey")
            .WithSummary("Перевыпустить токен клиентского пользователя")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.PlaceUser.CanEdit);

        group.MapDelete("/{placeUserId:guid}", Delete)
            .WithName("DeletePlaceUser")
            .WithSummary("Удалить клиентского пользователя")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.PlaceUser.CanDelete);
    }

    internal async Task<IResult> GetAll(IPlaceUserService service, ClaimsPrincipal user)
    {
        var users = await service.GetAllAsync();
        return Results.Ok(users);
    }

    internal async Task<IResult> GetById(IPlaceUserService service, Guid placeUserId)
    {
        var user = await service.GetByIdAsync(placeUserId);
        return user is null ? Results.NotFound() : Results.Ok(new PlaceUserDto
        {
            Id = user.Id,
            Name = user.Name,
            ModifiedId = user.ModifiedId, 
        });
    }

    internal async Task<IResult> Create(IPlaceUserService service, CreatePlaceUserRequest request)
    {
        var result = await service.CreateAsync(request);
        return Results.Ok(result);
    }

    internal async Task<IResult> Update(IPlaceUserService service, Guid placeUserId, UpdatePlaceUserRequest request)
    {
        await service.UpdateAsync(placeUserId, request);
        return Results.Ok();
    }
    internal async Task<IResult> UpdateKey(IPlaceUserService service, Guid placeUserId)
    {
        var result = await service.UpdateJWT(placeUserId);
        return Results.Ok(result);
    }
    internal async Task<IResult> Delete(IPlaceUserService service, Guid placeUserId)
    {
        var user = await service.GetByIdAsync(placeUserId);
        if (user is null)
            return Results.NotFound();

        await service.DeleteAsync(placeUserId);
        return Results.Ok();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IPlaceUserService, PlaceUserService>();
    }
}