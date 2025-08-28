using ASW.RemoteViewing.Features.RemoteDriver.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.RemoteDriver;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Requests.RemoteDriver;

namespace ASW.RemoteViewing.Features.RemoteDriver.Endpoints;

public class RemoteDriverEndpointDefinition : IEndpointDefinition
{
    private const string BaseRoute = "/api/v1/ReferenceBooks/Drivers";

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(BaseRoute, GetAllAsync)
            .WithName("GetAllRemoteDrivers")
            .WithSummary("Получить всех водителей")
            .Produces<List<RemoteDriverDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemoteDriver.CanView);

        app.MapGet($"{BaseRoute}/{{driverId:guid}}", GetByIdAsync)
            .WithName("GetRemoteDriverById")
            .WithSummary("Получить водителя по ID")
            .Produces<RemoteDriverDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(Policies.RemoteDriver.CanView);

        app.MapPost(BaseRoute, CreateAsync)
            .WithName("CreateRemoteDriver")
            .WithSummary("Создать нового водителя")
            .Accepts<AddRemoteDriverRequest>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteDriver.CanAdd);

        app.MapPut(BaseRoute, UpdateAsync)
            .WithName("UpdateRemoteDriver")
            .WithSummary("Обновить водителя")
            .Accepts<UpdateRemoteDriverRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteDriver.CanEdit);

        app.MapDelete($"{BaseRoute}/{{driverId:guid}}", DeleteAsync)
            .WithName("DeleteRemoteDriver")
            .WithSummary("Удалить водителя по ID")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteDriver.CanDelete);
    }

    internal async Task<IResult> GetAllAsync(IRemoteDriverService service)
    {
        var result = await service.GetAllAsync();
        return Results.Ok(result);
    }

    internal async Task<IResult> GetByIdAsync(IRemoteDriverService service, Guid driverId)
    {
        var result = await service.GetByIdAsync(driverId);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    internal async Task<IResult> CreateAsync(
        IRemoteDriverService service,
        AddRemoteDriverRequest request)
    {
        var result = await service.CreateAsync(request);
        return Results.Created($"{BaseRoute}/{request.Id}", result);
    }

    internal async Task<IResult> UpdateAsync(
        IRemoteDriverService service,
        UpdateRemoteDriverRequest request)
    {
        await service.UpdateAsync(request);
        return Results.NoContent();
    }

    internal async Task<IResult> DeleteAsync(IRemoteDriverService service, Guid driverId)
    {
        await service.DeleteAsync(driverId);
        return Results.Ok();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRemoteDriverService, RemoteDriverService>();
    }
}
