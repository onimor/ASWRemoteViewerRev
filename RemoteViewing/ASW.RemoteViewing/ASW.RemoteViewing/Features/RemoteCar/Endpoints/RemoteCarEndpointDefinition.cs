using ASW.RemoteViewing.Features.RemoteCar.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.RemoteCar;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Requests.RemoteCar;
using Microsoft.AspNetCore.Mvc;

namespace ASW.RemoteViewing.Features.RemoteCar.Endpoints;

public class RemoteCarEndpointDefinition : IEndpointDefinition
{
    private const string BaseRoute = "/api/v1/ReferenceBooks/Cars";

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet(BaseRoute, GetAsync)
            .WithName("GetCars")
            .WithSummary("Получить список всех машин или машину по номеру")
            .WithDescription("Если указан query-параметр 'number', вернёт одну машину по номеру. Иначе — все машины.")
            .Produces<List<RemoteCarDto>>(StatusCodes.Status200OK)
            .Produces<RemoteCarDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(Policies.RemoteCar.CanView);

        app.MapGet($"{BaseRoute}/{{carId:guid}}", GetByIdAsync)
            .WithName("GetCarById")
            .WithSummary("Получить машину по ID")
            .Produces<RemoteCarDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(Policies.RemoteCar.CanView);

        app.MapPost(BaseRoute, CreateAsync)
            .WithName("CreateRemoteCar")
            .WithSummary("Создать новую машину")
            .Accepts<AddRemoteCarRequest>("application/json")
            .Produces<AddRemoteCarRequest>(StatusCodes.Status201Created)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteCar.CanAdd);

        app.MapPut(BaseRoute, UpdateAsync)
            .WithName("UpdateRemoteCar")
            .WithSummary("Обновить информацию о машине")
            .Accepts<UpdateRemoteCarRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteCar.CanEdit);

        app.MapDelete($"{BaseRoute}/{{carId:guid}}", DeleteAsync)
            .WithName("DeleteRemoteCar")
            .WithSummary("Удалить машину по ID")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteCar.CanDelete);
    }

    internal async Task<IResult> GetAsync(
        IRemoteCarService service,
        [FromQuery] string? number)
    {
        if (!string.IsNullOrWhiteSpace(number))
        {
            var car = await service.GetByNumber(number);
            return car is null ? Results.NotFound() : Results.Ok(car);
        } 
        var all = await service.GetAllAsync();
        return Results.Ok(all);
    }

    internal async Task<IResult> GetByIdAsync(IRemoteCarService service, Guid carId)
    {
        var car = await service.GetByIdAsync(carId);
        return car is null ? Results.NotFound() : Results.Ok(car);
    }

    internal async Task<IResult> CreateAsync(
        IRemoteCarService service,
        AddRemoteCarRequest request)
    {
        var result = await service.CreateAsync(request);
        return Results.Created($"{BaseRoute}/{result.Id}", result);
    }

    internal async Task<IResult> UpdateAsync(
        IRemoteCarService service,
        UpdateRemoteCarRequest request)
    {
        await service.UpdateAsync(request);
        return Results.NoContent();
    }

    internal async Task<IResult> DeleteAsync(
        IRemoteCarService service,
        Guid carId)
    {
        await service.DeleteAsync(carId);
        return Results.Ok();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRemoteCarService, RemoteCarService>();
    }
}
