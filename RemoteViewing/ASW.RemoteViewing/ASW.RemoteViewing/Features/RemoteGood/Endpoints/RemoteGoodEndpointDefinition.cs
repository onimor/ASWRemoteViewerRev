using ASW.RemoteViewing.Features.RemoteGood.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.RemoteGood;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Requests.RemoteGood;

namespace ASW.RemoteViewing.Features.RemoteGood.Endpoints;

public class RemoteGoodEndpointDefinition : IEndpointDefinition
{
    private const string BaseRoute = "/api/v1/ReferenceBooks/Goods";

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(BaseRoute, GetAllAsync)
            .WithName("GetAllRemoteGoods")
            .WithSummary("Получить все товары")
            .Produces<List<RemoteGoodDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemoteGood.CanView);

        app.MapGet($"{BaseRoute}/{{goodId:guid}}", GetByIdAsync)
            .WithName("GetRemoteGoodById")
            .WithSummary("Получить товар по ID")
            .Produces<RemoteGoodDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(Policies.RemoteGood.CanView);

        app.MapPost(BaseRoute, CreateAsync)
            .WithName("CreateRemoteGood")
            .WithSummary("Создать товар")
            .Accepts<AddRemoteGoodRequest>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteGood.CanAdd);

        app.MapPut(BaseRoute, UpdateAsync)
            .WithName("UpdateRemoteGood")
            .WithSummary("Обновить товар")
            .Accepts<UpdateRemoteGoodRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteGood.CanEdit);

        app.MapDelete($"{BaseRoute}/{{goodId:guid}}", DeleteAsync)
            .WithName("DeleteRemoteGood")
            .WithSummary("Удалить товар по ID")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteGood.CanDelete);
    }

    internal async Task<IResult> GetAllAsync(IRemoteGoodService service)
    {
        var result = await service.GetAllAsync();
        return Results.Ok(result);
    }

    internal async Task<IResult> GetByIdAsync(IRemoteGoodService service, Guid goodId)
    {
        var result = await service.GetByIdAsync(goodId);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    internal async Task<IResult> CreateAsync(
        IRemoteGoodService service,
        AddRemoteGoodRequest request)
    {
        var result = await service.CreateAsync(request);
        return Results.Created($"{BaseRoute}/{request.Id}", result);
    }

    internal async Task<IResult> UpdateAsync(
        IRemoteGoodService service,
        UpdateRemoteGoodRequest request)
    {
        await service.UpdateAsync(request);
        return Results.NoContent();
    }

    internal async Task<IResult> DeleteAsync(IRemoteGoodService service, Guid goodId)
    {
        await service.DeleteAsync(goodId);
        return Results.Ok();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRemoteGoodService, RemoteGoodService>();
    }
}
