using ASW.RemoteViewing.Features.RemoteWeighing.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.RemoteWeighing;
using ASW.RemoteViewing.Shared.Requests.RemoteWeighing;
using ASW.RemoteViewing.Shared.Security;
using ASW.RemoteViewing.Shared.Utilities.ModelBinding;
using ASW.Shared.Extentions;
using ASW.Shared.Requests.RemoteWeighing;
using Microsoft.AspNetCore.Mvc;

namespace ASW.RemoteViewing.Features.RemoteWeighing.Endpoints;

public class RemoteWeighingEndpointDefinition : IEndpointDefinition
{
    private const string BaseRoute = "/api/v1/Remote/Posts";

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet($"{BaseRoute}/Weighings", GetAllAsync)
            .WithName("GetAllRemoteWeighings")
            .WithSummary("Получить все взвешивания")
            .WithDescription("Возвращает список всех взвешиваний всех постов")
            .Produces<List<RemoteWeighingDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemoteWeighing.CanView);

        app.MapGet($"{BaseRoute}/Weighings/{{weighingId:guid}}", GetByIdAsync)
            .WithName("GetRemoteWeighingById")
            .WithSummary("Получить взвешивание по Id")
            .WithDescription("Возвращает взвешивание по идентификатору")
            .Produces<RemoteWeighingDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(Policies.RemoteWeighing.CanView);

        app.MapGet($"{BaseRoute}/{{postId:guid}}/Weighings", GetByPostAndDateAsync)
            .WithName("GetRemoteWeighingsByPostAndDate")
            .WithSummary("Получить взвешивания по посту и дате")
            .WithDescription("Возвращает список взвешиваний за указанный диапазон дат по заданному посту")
            .Produces<List<RemoteWeighingDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemoteWeighing.CanView);

        app.MapGet($"{BaseRoute}/{{postId:guid}}/Weighings/Last", GetLastOneWeighingAsync)
            .WithName("GetLastRemoteWeighing")
            .WithSummary("Получить последнее взвешивание по посту")
            .WithDescription("Возвращает последнее взвешивание для поста")
            .Produces<RemoteWeighingDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(Policies.RemoteWeighing.CanView);

        app.MapGet($"{BaseRoute}/Weighings/ByDate", GetByDateAsync)
            .WithName("GetRemoteWeighingsByDate")
            .WithSummary("Получить взвешивания за период")
            .WithDescription("Возвращает список взвешиваний в диапазоне дат по всем постам")
            .Produces<List<RemoteWeighingDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemoteWeighing.CanView);

        app.MapPost($"{BaseRoute}/Weighings", CreateAsync)
            .WithName("CreateRemoteWeighing")
            .WithSummary("Создать взвешивание")
            .WithDescription("Создаёт новое взвешивание")
            .Accepts<AddRemoteWeighingRequest>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteWeighing.CanAdd);

        app.MapPut($"{BaseRoute}/Weighings/{{weighingId:guid}}", UpdateAsync)
            .WithName("UpdateRemoteWeighing")
            .WithSummary("Обновить взвешивание")
            .WithDescription("Обновляет существующее взвешивание")
            .Accepts<UpdateRemoteWeighingRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteWeighing.CanEdit);

        app.MapDelete($"{BaseRoute}/Weighings/{{weighingId:guid}}", DeleteAsync)
            .WithName("DeleteRemoteWeighing")
            .WithSummary("Удалить взвешивание")
            .WithDescription("Удаляет взвешивание по идентификатору")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemoteWeighing.CanDelete);
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRemoteWeighingService, RemoteWeighingService>();
    }

    internal async Task<IResult> GetAllAsync(IRemoteWeighingService service)
    {
        var result = await service.GetAllAsync();
        return Results.Ok(result);
    }
    
    internal async Task<IResult> GetByIdAsync(IRemoteWeighingService service, Guid weighingId)
    {
        var result = await service.GetByIdAsync(weighingId);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }

    internal async Task<IResult> CreateAsync(IRemoteWeighingService service, AddRemoteWeighingRequest request)
    {
        var created = await service.CreateAsync(request);
        return Results.Created($"{BaseRoute}/Weighings", created);
    }

    internal async Task<IResult> UpdateAsync(IRemoteWeighingService service, Guid weighingId, UpdateRemoteWeighingRequest request)
    {
        if (weighingId != request.Id)
            return Results.BadRequest("Id в URL и теле запроса не совпадают");

        await service.UpdateAsync(request);
        return Results.NoContent();
    }

    internal async Task<IResult> DeleteAsync(IRemoteWeighingService service, Guid weighingId)
    {
        await service.DeleteAsync(weighingId);
        return Results.Ok();
    }


    internal async Task<IResult> GetLastOneWeighingAsync(IRemoteWeighingService service, [FromQuery] Guid postId)
    {
        var result = await service.GetLastOneWeighingAsync(postId);
        return result is null ? Results.NotFound() : Results.Ok(result);
    }
    internal async Task<IResult> GetByPostAndDateAsync(
        IRemoteWeighingService service,
       [FromQuery] Guid postId,
       [FromQuery] StrictIso8601UtcDateTime? DateStart,
       [FromQuery] StrictIso8601UtcDateTime? DateEnd)
    {
        if (DateStart is null || DateEnd is null)
            throw new ValidationException("Неверный формат даты. Используйте ISO 8601 UTC формат: yyyy-MM-ddTHH:mm:ssZ");
       
        var result = await service.GetByPostAndDateAsync(new RemoteWeightByDateRequest
        {
            DateStart = DateStart?.UtcDateTime,
            DateEnd = DateEnd?.UtcDateTime,
        }, postId);
        return Results.Ok(result);
    }

    internal async Task<IResult> GetByDateAsync(
        IRemoteWeighingService service,
       [FromQuery] StrictIso8601UtcDateTime? DateStart,
       [FromQuery] StrictIso8601UtcDateTime? DateEnd)
    {
        if (DateStart is null || DateEnd is null)
            throw new ValidationException("Неверный формат даты. Используйте ISO 8601 UTC формат: yyyy-MM-ddTHH:mm:ssZ");
        
        var result = await service.GetByDateAsync(new RemoteWeightByDateRequest
        {
            DateStart = DateStart?.UtcDateTime,
            DateEnd = DateEnd?.UtcDateTime,
        });
        return Results.Ok(result);
    }
}
