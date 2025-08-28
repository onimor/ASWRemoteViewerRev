using ASW.RemoteViewing.Features.RemotePostUsers.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.RemotePostUsers;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Requests.RemotePostUsers;

namespace ASW.RemoteViewing.Features.RemotePostUsers.Endpoints;

public class RemotePostUsersEndpointDefinition : IEndpointDefinition
{
    private const string BaseRoute = "/api/v1/Remote/Posts";

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet($"{BaseRoute}/{{postId:guid}}/Users", GetByPostAsync)
            .WithName("GetPostUsersByPost")
            .WithSummary("Получить пользователей, привязанных к посту")
            .WithDescription("Возвращает всех пользователей, связанных с указанным постом")
            .Produces<List<RemotePostUserDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemotePostUsers.CanView);

        app.MapGet("/api/v1/Remote/Users/{userId:guid}/Posts", GetByUserAsync)
            .WithName("GetPostUsersByUser")
            .WithSummary("Получить посты, связанные с пользователем")
            .WithDescription("Возвращает все посты, связанные с указанным пользователем")
            .Produces<List<RemotePostUserDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemotePostUsers.CanView);

        app.MapPost($"{BaseRoute}/{{postId:guid}}/Users", CreateAsync)
            .WithName("AddUsersToPost")
            .WithSummary("Добавить пользователей к посту")
            .WithDescription("Добавляет пользователей к заданному посту")
            .Accepts<AddRemotePostUsersRequest>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemotePostUsers.CanAdd);

        app.MapDelete($"{BaseRoute}/{{postId:guid}}/Users", DeleteByPostAsync)
            .WithName("DeletePostUsersByPost")
            .WithSummary("Удалить всех пользователей, связанных с постом")
            .WithDescription("Удаляет всех пользователей, привязанных к посту")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemotePostUsers.CanDelete);
    }

    private async Task<IResult> GetByPostAsync(IRemotePostUsersService service, Guid postId)
    {
        var result = await service.GetByPostAsync(postId);
        return Results.Ok(result);
    }

    private async Task<IResult> CreateAsync(
        IRemotePostUsersService service,
        AddRemotePostUsersRequest request,
        Guid postId)
    {
        if (postId != request.PostId)
            return Results.BadRequest("PostId в теле запроса не совпадает с URL");

        await service.CreateAsync(request);
        return Results.Created($"{BaseRoute}/{postId}/Users", null);
    }

    private async Task<IResult> DeleteByPostAsync(IRemotePostUsersService service, Guid postId)
    {
        await service.DeleteByPostAsync(postId);
        return Results.Ok();
    }

    private async Task<IResult> GetByUserAsync(IRemotePostUsersService service, Guid userId)
    {
        var result = await service.GetByUserAsync(userId);
        return Results.Ok(result);
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRemotePostUsersService, RemotePostUsersService>();
    }
}
