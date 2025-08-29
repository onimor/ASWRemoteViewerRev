using ASW.RemoteViewing.Features.RemoteAxesDist.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.RemoteAxes;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Extension;
using ASW.Shared.Requests.RemoteAxesDist;

namespace ASW.RemoteViewing.Features.RemoteAxesDist.Endpoints;

public class RemoteAxesDistEndpointDefinition : IEndpointDefinition
{
    private const string Route = "/api/v1/Remote/Posts/Weighing/{id}/Axes/Dist";

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(Route, GetAsync)
            .WithName("GetAxesDist")
            .WithSummary("Получить межосевую дистанцию по взвешиванию")
            .WithDescription("Возвращает межосевую дистанцию по ID взвешивания.")
            .Produces<RemoteAxesDistDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(Policies.RemoteAxesDist.CanView);

        app.MapPost(Route, CreateAsync)
            .WithName("CreateRemoteAxesDist")
            .WithSummary("Создать межосевую дистанцию")
            .WithDescription("Создаёт межосевую дистанцию для указанного взвешивания.")
            .Accepts<AddRemoteAxesDistRequest>("application/json")
            .Produces<RemoteAxesDistDto>(StatusCodes.Status201Created)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteAxesDist.CanAdd);

        app.MapDelete(Route, DeleteAsync)
            .WithName("DeleteRemoteAxesDist")
            .WithSummary("Удалить межосевую дистанцию")
            .WithDescription("Удаляет межосевую дистанцию, привязанную к взвешиванию.")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteAxesDist.CanDelete);
    }

    internal async Task<IResult> GetAsync(IRemoteAxesDistService service, Guid id)
    {
        var remoteAxesDist = await service.GetByWeighingAsync(id);
        return remoteAxesDist is null
            ? Results.NotFound()
            : Results.Ok(remoteAxesDist);
    }

    internal async Task<IResult> CreateAsync(
        IRemoteAxesDistService service,
        AddRemoteAxesDistRequest addRequest,
        Guid id)
    {
        if (id != addRequest.WeighingId)
            throw new ValidationException("Неверный ID взвешивания");

        var result = await service.CreateAsync(addRequest);
        return Results.Created($"/api/v1/Remote/Weighing/{id}/Axes/Dist", result);
    }

    internal async Task<IResult> DeleteAsync(IRemoteAxesDistService service, Guid id)
    {
        await service.DeleteAsync(id);
        return Results.Ok();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRemoteAxesDistService, RemoteAxesDistService>();
    }
}
