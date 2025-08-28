using ASW.RemoteViewing.Features.RemoteTrailer.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.RemoteTrailer;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Requests.RemoteTrailer;

namespace ASW.RemoteViewing.Features.RemoteTrailer.Endpoints;

public class RemoteTrailerEndpointDefinition : IEndpointDefinition
{
    private const string BaseRoute = "/api/v1/ReferenceBooks/Trailers";

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(BaseRoute, GetAllAsync)
            .WithName("GetAllRemoteTrailers")
            .WithSummary("Получить все прицепы")
            .Produces<List<RemoteTrailerDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemoteTrailer.CanView);

        app.MapGet($"{BaseRoute}/{{trailerId:guid}}", GetByIdAsync)
            .WithName("GetRemoteTrailerById")
            .WithSummary("Получить прицеп по ID")
            .Produces<RemoteTrailerDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(Policies.RemoteTrailer.CanView);

        app.MapPost(BaseRoute, CreateAsync)
            .WithName("CreateRemoteTrailer")
            .WithSummary("Создать прицеп")
            .Accepts<AddRemoteTrailerRequest>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteTrailer.CanAdd);

        app.MapPut(BaseRoute, UpdateAsync)
            .WithName("UpdateRemoteTrailer")
            .WithSummary("Обновить прицеп")
            .Accepts<UpdateRemoteTrailerRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteTrailer.CanEdit);

        app.MapDelete($"{BaseRoute}/{{trailerId:guid}}", DeleteAsync)
            .WithName("DeleteRemoteTrailer")
            .WithSummary("Удалить прицеп по ID")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteTrailer.CanDelete);
    }

    internal async Task<IResult> GetAllAsync(IRemoteTrailerService service)
    {
        var result = await service.GetAllAsync();
        return Results.Ok(result);
    }

    internal async Task<IResult> GetByIdAsync(IRemoteTrailerService service, Guid trailerId)
    {
        var result = await service.GetByIdAsync(trailerId);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    internal async Task<IResult> CreateAsync(
        IRemoteTrailerService service,
        AddRemoteTrailerRequest request)
    {
        var result = await service.CreateAsync(request);
        return Results.Created($"{BaseRoute}/{request.Id}", result);
    }

    internal async Task<IResult> UpdateAsync(
        IRemoteTrailerService service,
        UpdateRemoteTrailerRequest request)
    {
        await service.UpdateAsync(request);
        return Results.NoContent();
    }

    internal async Task<IResult> DeleteAsync(IRemoteTrailerService service, Guid trailerId)
    {
        await service.DeleteAsync(trailerId);
        return Results.Ok();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRemoteTrailerService, RemoteTrailerService>();
    }
}
