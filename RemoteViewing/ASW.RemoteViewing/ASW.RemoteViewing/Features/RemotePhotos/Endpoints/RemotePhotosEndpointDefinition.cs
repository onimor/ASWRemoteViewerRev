using ASW.RemoteViewing.Features.RemotePhotos.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.RemotePhoto;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Requests.RemoteEmptyWeighingPhoto;
using ASW.Shared.Requests.RemotePhotoBrutto;
using ASW.Shared.Requests.RemotePhotoTara;

namespace ASW.RemoteViewing.Features.RemotePhotos.Endpoints;

public class RemotePhotosEndpointDefinition : IEndpointDefinition
{
    private const string WeighingBaseRoute = "/api/v1/Remote/Posts/Weighings/{weighingId:guid}/Photos";
    private const string EmptyWeighingBaseRoute = "/api/v1/Remote/Posts/EmptyWeighings/{emptyWeighingId:guid}/Photos";

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet($"{WeighingBaseRoute}/Tara", GetTaraPhotosAsync)
            .WithName("GetTaraPhotos")
            .WithSummary("Получить фото тары")
            .WithDescription("Получает список фото тары по ID взвешивания.")
            .Produces<List<RemotePhotoTaraDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemotePhotos.CanView);

        app.MapPost($"{WeighingBaseRoute}/Tara", AddTaraPhotoAsync)
            .WithName("AddTaraPhoto")
            .WithSummary("Добавить фото тары")
            .WithDescription("Добавляет фото тары к взвешиванию.")
            .Accepts<AddRemotePhotoTaraRequest>("application/json")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemotePhotos.CanAdd);

        app.MapDelete($"{WeighingBaseRoute}/Tara", DeleteTaraPhotosAsync)
            .WithName("DeleteTaraPhotos")
            .WithSummary("Удалить фото тары")
            .WithDescription("Удаляет все фото тары, связанные с указанным ID взвешивания.")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemotePhotos.CanDelete);

        app.MapGet($"{WeighingBaseRoute}/Brutto", GetBruttoPhotosAsync)
            .WithName("GetBruttoPhotos")
            .WithSummary("Получить фото брутто")
            .WithDescription("Получает список фото брутто по ID взвешивания.")
            .Produces<List<RemotePhotoBruttoDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemotePhotos.CanView);

        app.MapPost($"{WeighingBaseRoute}/Brutto", AddBruttoPhotoAsync)
            .WithName("AddBruttoPhoto")
            .WithSummary("Добавить фото брутто")
            .WithDescription("Добавляет фото брутто к взвешиванию.")
            .Accepts<AddRemotePhotoBruttoRequest>("application/json")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemotePhotos.CanAdd);

        app.MapDelete($"{WeighingBaseRoute}/Brutto", DeleteBruttoPhotosAsync)
            .WithName("DeleteBruttoPhotos")
            .WithSummary("Удалить фото брутто")
            .WithDescription("Удаляет все фото брутто, связанные с указанным ID взвешивания.")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemotePhotos.CanDelete);

        app.MapGet(EmptyWeighingBaseRoute, GetEmptyWeighingPhotosAsync)
            .WithName("GetEmptyWeighingPhotos")
            .WithSummary("Получить фото порожнего проезда")
            .WithDescription("Получает список фото порожнего проезда по ID взвешивания.")
            .Produces<List<RemoteEmptyWeighingPhotoDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemotePhotos.CanView);

        app.MapPost(EmptyWeighingBaseRoute, AddEmptyWeighingPhotoAsync)
            .WithName("AddEmptyWeighingPhoto")
            .WithSummary("Добавить фото порожнего проезда")
            .WithDescription("Добавляет фото к порожнему проезду.")
            .Accepts<AddRemoteEmptyWeighingPhotoRequest>("application/json")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemotePhotos.CanAdd);

        app.MapDelete(EmptyWeighingBaseRoute, DeleteEmptyWeighingPhotosAsync)
            .WithName("DeleteEmptyWeighingPhotos")
            .WithSummary("Удалить фото порожнего проезда")
            .WithDescription("Удаляет все фото порожнего проезда по ID взвешивания.")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemotePhotos.CanDelete);
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRemotePhotosService, RemotePhotosService>();
    }

    private async Task<IResult> AddTaraPhotoAsync(IRemotePhotosService service, AddRemotePhotoTaraRequest request)
    {
        await service.AddTaraPhotoAsync(request);
        return Results.Ok();
    }

    private async Task<IResult> GetTaraPhotosAsync(IRemotePhotosService service, Guid weighingId)
    {
        var photos = await service.GetTaraPhotoAsync(weighingId);
        return Results.Ok(photos);
    }

    private async Task<IResult> DeleteTaraPhotosAsync(IRemotePhotosService service, Guid weighingId)
    {
        await service.DeleteTaraPhotosAsync(weighingId);
        return Results.Ok();
    }

    private async Task<IResult> AddBruttoPhotoAsync(IRemotePhotosService service, AddRemotePhotoBruttoRequest request)
    {
        await service.AddBruttoPhotoAsync(request);
        return Results.Ok();
    }

    private async Task<IResult> GetBruttoPhotosAsync(IRemotePhotosService service, Guid weighingId)
    {
        var photos = await service.GetBruttoPhotoAsync(weighingId);
        return Results.Ok(photos);
    }

    private async Task<IResult> DeleteBruttoPhotosAsync(IRemotePhotosService service, Guid weighingId)
    {
        await service.DeleteBruttoPhotosAsync(weighingId);
        return Results.Ok();
    }

    private async Task<IResult> AddEmptyWeighingPhotoAsync(IRemotePhotosService service, AddRemoteEmptyWeighingPhotoRequest request)
    {
        await service.AddEmptyWeighingPhotoAsync(request);
        return Results.Ok();
    }

    private async Task<IResult> GetEmptyWeighingPhotosAsync(IRemotePhotosService service, Guid emptyWeighingId)
    {
        var photos = await service.GetEmptyWeighingPhotoAsync(emptyWeighingId);
        return Results.Ok(photos);
    }

    private async Task<IResult> DeleteEmptyWeighingPhotosAsync(IRemotePhotosService service, Guid emptyWeighingId)
    {
        await service.DeleteEmptyWeighingPhotosAsync(emptyWeighingId);
        return Results.Ok();
    }
}
