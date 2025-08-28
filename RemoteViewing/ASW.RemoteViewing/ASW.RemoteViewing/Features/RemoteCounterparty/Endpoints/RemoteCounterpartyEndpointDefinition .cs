using ASW.RemoteViewing.Features.RemoteCounterparty.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.RemoteCounterparty;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Requests.RemoteCounterparty;

namespace ASW.RemoteViewing.Features.RemoteCounterparty.Endpoints;

public class RemoteCounterpartyEndpointDefinition : IEndpointDefinition
{
    private const string BaseRoute = "/api/v1/ReferenceBooks/Counterparties";

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(BaseRoute, GetAllAsync)
            .WithName("GetAllRemoteCounterparties")
            .WithSummary("Получить всех контрагентов")
            .Produces<List<RemoteCounterpartyDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemoteCounterparty.CanView);

        app.MapGet($"{BaseRoute}/{{counterpartyId:guid}}", GetByIdAsync)
            .WithName("GetRemoteCounterpartyById")
            .WithSummary("Получить контрагента по ID")
            .Produces<RemoteCounterpartyDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(Policies.RemoteCounterparty.CanView);

        app.MapPost(BaseRoute, CreateAsync)
            .WithName("CreateRemoteCounterparty")
            .WithSummary("Создать нового контрагента")
            .Accepts<AddRemoteCounterpartyRequest>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteCounterparty.CanAdd);

        app.MapPut(BaseRoute, UpdateAsync)
            .WithName("UpdateRemoteCounterparty")
            .WithSummary("Обновить контрагента")
            .Accepts<UpdateRemoteCounterpartyRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteCounterparty.CanEdit);

        app.MapDelete($"{BaseRoute}/{{counterpartyId:guid}}", DeleteAsync)
            .WithName("DeleteRemoteCounterparty")
            .WithSummary("Удалить контрагента по ID")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteCounterparty.CanDelete);
    }

    internal async Task<IResult> GetAllAsync(IRemoteCounterpartyService service)
    {
        var result = await service.GetAllAsync();
        return Results.Ok(result);
    }

    internal async Task<IResult> GetByIdAsync(
        IRemoteCounterpartyService service,
        Guid counterpartyId)
    {
        var result = await service.GetByIdAsync(counterpartyId);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    internal async Task<IResult> CreateAsync(
        IRemoteCounterpartyService service,
        AddRemoteCounterpartyRequest request)
    {
        var result = await service.CreateAsync(request);
        return Results.Created($"{BaseRoute}/{request.Id}", result);
    }

    internal async Task<IResult> UpdateAsync(
        IRemoteCounterpartyService service,
        UpdateRemoteCounterpartyRequest request)
    {
        await service.UpdateAsync(request);
        return Results.NoContent();
    }

    internal async Task<IResult> DeleteAsync(
        IRemoteCounterpartyService service,
        Guid counterpartyId)
    {
        await service.DeleteAsync(counterpartyId);
        return Results.Ok();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRemoteCounterpartyService, RemoteCounterpartyService>();
    }
}
