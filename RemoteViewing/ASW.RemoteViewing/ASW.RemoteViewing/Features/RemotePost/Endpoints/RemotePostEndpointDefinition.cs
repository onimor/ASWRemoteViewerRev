using ASW.RemoteViewing.Features.RemotePost.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.RemotePost;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Requests.RemotePost;

namespace ASW.RemoteViewing.Features.RemotePost.Endpoints;

public class RemotePostEndpointDefinition : IEndpointDefinition
{
    private const string BaseRoute = "/api/v1/Posts";

    public void DefineEndpoints(WebApplication app)
    {
        app.MapGet($"{BaseRoute}", GetAllAsync)
            .WithName("GetAllRemotePosts")
            .WithSummary("Получить все посты")
            .Produces<List<RemotePostDto>>(StatusCodes.Status200OK)
            .RequireAuthorization(Policies.RemotePost.CanView);

        app.MapGet($"{BaseRoute}/{{id:guid}}", GetByIdAsync)
            .WithName("GetRemotePostById")
            .WithSummary("Получить пост по ID")
            .Produces<RemotePostDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .RequireAuthorization(Policies.RemotePost.CanView);

        app.MapPost($"{BaseRoute}", CreateAsync)
            .WithName("CreateRemotePost")
            .WithSummary("Создать новый пост")
            .Accepts<AddRemotePostRequest>("application/json")
            .Produces(StatusCodes.Status201Created)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemotePost.CanAdd);

        app.MapPut($"{BaseRoute}", UpdateAsync)
            .WithName("UpdateRemotePost")
            .WithSummary("Обновить пост")
            .Accepts<UpdateRemotePostRequest>("application/json")
            .Produces(StatusCodes.Status204NoContent)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemotePost.CanEdit);

        app.MapDelete($"{BaseRoute}/{{id:guid}}", DeleteAsync)
            .WithName("DeleteRemotePost")
            .WithSummary("Удалить пост по ID")
            .Produces(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.RemotePost.CanDelete);
    }

    private async Task<IResult> GetAllAsync(IRemotePostService service)
    {
        var posts = await service.GetAllAsync();
        return Results.Ok(posts);
    }

    private async Task<IResult> GetByIdAsync(IRemotePostService service, Guid id)
    {
        var post = await service.GetByIdAsync(id);
        return post is null ? Results.NotFound() : Results.Ok(post);
    }

    private async Task<IResult> CreateAsync(IRemotePostService service, AddRemotePostRequest request)
    {
        var result = await service.CreateAsync(request);
        return Results.Created($"/api/v1/Posts/{result.Id}", result);
    }

    private async Task<IResult> UpdateAsync(IRemotePostService service, UpdateRemotePostRequest request)
    {
        await service.UpdateAsync(request);
        return Results.NoContent();
    }

    private async Task<IResult> DeleteAsync(IRemotePostService service, Guid id)
    {
        await service.DeleteAsync(id);
        return Results.Ok();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IRemotePostService, RemotePostService>();
    }
}
