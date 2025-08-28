using ASW.RemoteViewing.Features.RemoteCamera.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.RemoteCamera;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Requests.RemoteCamera;

namespace ASW.RemoteViewing.Features.RemoteCamera.Endpoints;

public class RemoteCameraEndpointDefinition : IEndpointDefinition
{
    private const string BaseRoute = "/api/v1/Remote/Posts/Cameras";

    public void DefineEndpoints(WebApplication app)
    {
        // Получить все камеры
        app.MapGet(BaseRoute, GetAllAsync)
            .WithName("GetAllRemoteCameras")
            .WithSummary("Получить все камеры")
            .WithDescription("Возвращает все камеры без фильтра по посту")
            .Produces<List<RemoteCameraDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemoteCamera.CanView);

        // Получить камеры по посту
        app.MapGet("/api/v1/Remote/Posts/{postId:guid}/Cameras", GetAllByPostAsync)
            .WithName("GetAllRemoteCamerasByPost")
            .WithSummary("Получить камеры по посту")
            .Produces<List<RemoteCameraDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemoteCamera.CanView);

        // Получить одну камеру
        app.MapGet($"{BaseRoute}/{{cameraId:guid}}", GetByIdAsync)
            .WithName("GetRemoteCameraById")
            .WithSummary("Получить камеру по ID")
            .Produces<RemoteCameraDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(Policies.RemoteCamera.CanView);

        // Создание камеры
        app.MapPost(BaseRoute, CreateAsync)
            .WithName("CreateRemoteCamera")
            .Accepts<AddRemoteCameraRequest>("application/json")
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteCamera.CanAdd);

        // Обновление камеры
        app.MapPut($"{BaseRoute}/{{cameraId:guid}}", UpdateAsync)
            .WithName("UpdateRemoteCamera")
            .Accepts<UpdateRemoteCameraRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteCamera.CanEdit);

        // Удаление одной камеры
        app.MapDelete($"{BaseRoute}/{{cameraId:guid}}", DeleteAsync)
            .WithName("DeleteRemoteCamera")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteCamera.CanDelete);

        // Удаление всех камер по посту
        app.MapDelete("/api/v1/Remote/Posts/{postId:guid}/Cameras", DeleteByPostAsync)
            .WithName("DeleteAllRemoteCamerasByPost")
            .WithSummary("Удалить все камеры поста")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteCamera.CanDelete);
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRemoteCameraService, RemoteCameraService>();
    }

    internal async Task<IResult> GetAllAsync(IRemoteCameraService service)
    {
        var result = await service.GetAllAsync();
        return Results.Ok(result);
    }

    internal async Task<IResult> GetAllByPostAsync(IRemoteCameraService service, Guid postId)
    {
        var result = await service.GetAllByPostAsync(postId);
        return Results.Ok(result);
    }

    internal async Task<IResult> GetByIdAsync(IRemoteCameraService service, Guid cameraId)
    {
        var result = await service.GetByIdAsync(cameraId);
        return result == null ? Results.NotFound() : Results.Ok(result);
    }

    internal async Task<IResult> CreateAsync(IRemoteCameraService service, AddRemoteCameraRequest request)
    {
        var created = await service.CreateAsync(request);
        return Results.Created($"{BaseRoute}/{created.Id}", created);
    }

    internal async Task<IResult> UpdateAsync(IRemoteCameraService service, Guid cameraId, UpdateRemoteCameraRequest request)
    {
        if (cameraId != request.Id)
            return Results.BadRequest("CameraId в URL и теле запроса не совпадают");

        await service.UpdateAsync(request);
        return Results.NoContent();
    }

    internal async Task<IResult> DeleteAsync(IRemoteCameraService service, Guid cameraId)
    {
        var entity = await service.GetByIdAsync(cameraId);
        if (entity is null)
            return Results.NotFound();

        await service.DeleteAsync(cameraId);
        return Results.Ok();
    }

    internal async Task<IResult> DeleteByPostAsync(IRemoteCameraService service, Guid postId)
    {
        await service.DeleteByPostAsync(postId);
        return Results.Ok();
    }
}
