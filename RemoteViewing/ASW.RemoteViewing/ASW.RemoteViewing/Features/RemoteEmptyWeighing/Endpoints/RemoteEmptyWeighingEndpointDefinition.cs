using ASW.RemoteViewing.Features.RemoteEmptyWeighing.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.RemoteEmptyWeighing;
using ASW.RemoteViewing.Shared.Requests.RemoteWeighing;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Requests.RemoteEmptyWeighing;

namespace ASW.RemoteViewing.Features.RemoteEmptyWeighing.Endpoints;

public class RemoteEmptyWeighingEndpointDefinition : IEndpointDefinition
{
    private const string BaseRoute = "/api/v1/Remote/Posts/EmptyWeighings";

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(BaseRoute, GetAllOrByDateAsync)
            .WithName("GetEmptyWeighingsAllOrByDate")
            .WithSummary("Получить порожние проезды (все или по дате)")
            .WithDescription("Возвращает список всех порожних проездов или только за указанный период")
            .Produces<List<RemoteEmptyWeighingDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemoteEmptyWeighing.CanView);

        app.MapGet($"{BaseRoute}/{{id:guid}}", GetByIdAsync)
            .WithName("GetEmptyWeighingById")
            .WithSummary("Получить порожний проезд по ID")
            .WithDescription("Возвращает конкретный порожний проезд по идентификатору")
            .Produces<RemoteEmptyWeighingDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(Policies.RemoteEmptyWeighing.CanView);

        app.MapGet("/api/v1/Remote/Posts/{postId:guid}/EmptyWeighings", GetByPostAndDateAsync)
            .WithName("GetEmptyWeighingsByPostAndDate")
            .WithSummary("Получить порожние проезды по посту и дате")
            .WithDescription("Возвращает список порожних проездов по указанному посту и диапазону дат")
            .Produces<List<RemoteEmptyWeighingDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemoteEmptyWeighing.CanView);

        app.MapPost(BaseRoute, CreateAsync)
            .WithName("CreateEmptyWeighing")
            .WithSummary("Создать порожний проезд")
            .WithDescription("Создаёт новый порожний проезд")
            .Accepts<AddRemoteEmptyWeighingRequest>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ExcludeFromDescription()
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteEmptyWeighing.CanAdd);

        app.MapPut(BaseRoute, UpdateAsync)
            .WithName("UpdateEmptyWeighing")
            .WithSummary("Обновить порожний проезд")
            .WithDescription("Обновляет данные существующего порожнего проезда")
            .Accepts<UpdateRemoteEmptyWeighingRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ExcludeFromDescription()
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteEmptyWeighing.CanEdit);

        app.MapDelete($"{BaseRoute}/{{id:guid}}", DeleteAsync)
            .WithName("DeleteEmptyWeighing")
            .WithSummary("Удалить порожний проезд")
            .WithDescription("Удаляет порожний проезд по его ID")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteEmptyWeighing.CanDelete);
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRemoteEmptyWeighingService, RemoteEmptyWeighingService>();
    }

    private async Task<IResult> GetAllOrByDateAsync(
        IRemoteEmptyWeighingService service,
        [AsParameters] RemoteWeightByDateRequest query)
    {
        if (query.DateStart != default && query.DateEnd != default)
        {
            var result = await service.GetByDateAsync(query);
            return Results.Ok(result);
        }

        var all = await service.GetAllAsync();
        return Results.Ok(all);
    }

    private async Task<IResult> GetByPostAndDateAsync(
        IRemoteEmptyWeighingService service,
        Guid postId,
        [AsParameters] RemoteWeightByDateRequest query)
    {
        var result = await service.GetByPostAndDateAsync(query, postId);
        return Results.Ok(result);
    }

    private async Task<IResult> GetByIdAsync(IRemoteEmptyWeighingService service, Guid id)
    {
        var result = await service.GetByIdAsync(id);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    private async Task<IResult> CreateAsync(
        IRemoteEmptyWeighingService service,
        AddRemoteEmptyWeighingRequest request)
    {
        var result = await service.CreateAsync(request);
        return Results.Created($"{BaseRoute}/{result.Id}", result);
    }

    private async Task<IResult> UpdateAsync(
        IRemoteEmptyWeighingService service,
        UpdateRemoteEmptyWeighingRequest request)
    {
        await service.UpdateAsync(request);
        return Results.NoContent();
    }

    private async Task<IResult> DeleteAsync(IRemoteEmptyWeighingService service, Guid id)
    {
        await service.DeleteAsync(id);
        return Results.Ok();
    }
}
