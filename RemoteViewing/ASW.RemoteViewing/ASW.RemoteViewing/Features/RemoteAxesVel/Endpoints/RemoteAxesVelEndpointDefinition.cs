using ASW.RemoteViewing.Features.RemoteAxesVel.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.RemoteAxes;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Extentions;
using ASW.Shared.Requests.RemoteAxesVel;

namespace ASW.RemoteViewing.Features.RemoteAxesVel.Endpoints;

public class RemoteAxesVelEndpointDefinition : IEndpointDefinition
{
    private const string Route = "/api/v1/Remote/Posts/Weighing/{id}/Axes/Vel";
  
    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(Route, GetAsync)
            .WithName("GetRemoteAxesVel")
            .WithSummary("Получить осевые скорости по ID взвешивания")
            .WithDescription("Возвращает список осевых скоростей, привязанных к взвешиванию")
            .Produces<List<RemoteAxesVelDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(Policies.RemoteAxesVel.CanView);

        app.MapPost(Route, CreateAsync)
            .WithName("CreateRemoteAxesVel")
            .WithSummary("Создать осевые скорости для взвешивания")
            .WithDescription("Добавляет осевые скорости для указанного взвешивания")
            .Accepts<AddRemoteAxesVelRequest>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteAxesVel.CanAdd);

        app.MapDelete(Route, DeleteAsync)
            .WithName("DeleteRemoteAxesVel")
            .WithSummary("Удалить осевые скорости по ID взвешивания")
            .WithDescription("Удаляет все осевые скорости, привязанные к взвешиванию")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteAxesVel.CanDelete);
    }

    internal async Task<IResult> GetAsync(IRemoteAxesVelService service, Guid id)
    {
        var axes = await service.GetByWeighingAsync(id);
        return axes is null ? Results.NotFound() : Results.Ok(axes);
    }

    internal async Task<IResult> CreateAsync(
        IRemoteAxesVelService service,
        AddRemoteAxesVelRequest request,
        Guid id)
    {
        if (id != request.WeighingId)
            throw new ValidationException("Неверный ID взвешивания");

        await service.CreateAsync(request);
        return Results.Created($"/api/v1/Remote/Weighing/{id}/Axes/Vel", null);
    }

    internal async Task<IResult> DeleteAsync(IRemoteAxesVelService service, Guid id)
    {
        await service.DeleteAsync(id);
        return Results.Ok();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRemoteAxesVelService, RemoteAxesVelService>();
    }
}
