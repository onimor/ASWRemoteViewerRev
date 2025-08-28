using ASW.RemoteViewing.Features.RemoteAxesWeight.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.RemoteAxes;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Extentions;
using ASW.Shared.Requests.RemoteAxesWeight;

namespace ASW.RemoteViewing.Features.RemoteAxesWeight.Endpoints;

public class RemoteAxesWeightEndpointDefinition : IEndpointDefinition
{
    private const string Route = "/api/v1/Remote/Posts/Weighing/{id}/Axes/Weight";

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(Route, GetAsync)
            .WithName("GetRemoteAxesWeight")
            .WithSummary("Получить осевые веса по ID взвешивания")
            .WithDescription("Возвращает список веса осей, привязанных к взвешиванию")
            .Produces<List<RemoteAxesWeightDto>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(Policies.RemoteAxesWeight.CanView);

        app.MapPost(Route, CreateAsync)
            .WithName("CreateRemoteAxesWeight")
            .WithSummary("Создать вес оси для взвешивания")
            .WithDescription("Добавляет вес оси для указанного взвешивания")
            .Accepts<AddRemoteAxesWeightRequest>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteAxesWeight.CanAdd);

        app.MapDelete(Route, DeleteAsync)
            .WithName("DeleteRemoteAxesWeight")
            .WithSummary("Удалить вес осей по ID взвешивания")
            .WithDescription("Удаляет вес осей, привязанные к взвешиванию")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteAxesWeight.CanDelete);
    }

    internal async Task<IResult> GetAsync(IRemoteAxesWeightService service, Guid id)
    {
        var axes = await service.GetByWeighingAsync(id);
        return axes is null ? Results.NotFound() : Results.Ok(axes);
    }

    internal async Task<IResult> CreateAsync(
        IRemoteAxesWeightService service,
        AddRemoteAxesWeightRequest request,
        Guid id)
    {
        if (id != request.WeighingId)
            throw new ValidationException("Неверный ID взвешивания");

        await service.CreateAsync(request);
        return Results.Created($"/api/v1/Remote/Weighing/{id}/Axes/Weight", null);
    }

    internal async Task<IResult> DeleteAsync(IRemoteAxesWeightService service, Guid id)
    {
        await service.DeleteAsync(id);
        return Results.Ok();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRemoteAxesWeightService, RemoteAxesWeightService>();
    }
}
