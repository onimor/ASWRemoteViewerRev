using ASW.RemoteViewing.Features.User.Services;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Shared.Dto.User;
using ASW.RemoteViewing.Shared.Requests.User;
using ASW.RemoteViewing.Shared.Security;
using System.Security.Claims;

namespace ASW.RemoteViewing.Features.User.Endpoints;

public class UserEndpointDefinition : IEndpointDefinition
{
    public void DefineEndpoints(WebApplication app)
    {
        var group = app.MapGroup("/api/v1/Users")
            .WithTags("Users")
            .RequireAuthorization();

        group.MapGet("/", GetAll)
            .WithName("GetAllUsers")
            .WithSummary("Получить всех пользователей")
            .Produces<List<UserDto>>(StatusCodes.Status200OK)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.User.CanView);

        group.MapGet("/{userId:guid}", GetById)
            .WithName("GetUserById")
            .WithSummary("Получить пользователя по Id")
            .Produces<UserDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.User.CanView);

        group.MapPost("/", Create)
            .WithName("CreateUser")
            .WithSummary("Создание нового пользователя")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.User.CanAdd);

        group.MapPut("/{userId:guid}", Update)
            .WithName("UpdateUser")
            .WithSummary("Изменить пользователя")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.User.CanEdit);

        group.MapDelete("/{userId:guid}", Delete)
            .WithName("DeleteUser")
            .WithSummary("Удалить пользователя")
            .Produces(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .ExcludeFromDescription()
            .RequireAuthorization(Policies.User.CanDelete);
    }

    internal async Task<IResult> GetAll(IUserService service, ClaimsPrincipal user)
    {
        var users = await service.GetAll();
        return Results.Ok(users);
    }

    internal async Task<IResult> GetById(IUserService service, Guid userId)
    {
        var user = await service.GetById(userId);
        return user is null ? Results.NotFound() : Results.Ok(new UserDto
        {
            Id = user.Id,
            Login = user.Login,
            Role = user.Role,
            FIO = user.ReductionFIO
        });
    }

    internal async Task<IResult> Create(IUserService service, CreateUserRequest request)
    {
        await service.Create(request);
        return Results.Ok();
    }

    internal async Task<IResult> Update(IUserService service, Guid userId, UpdateUserRequest request)
    {
        await service.Update(userId, request);
        return Results.Ok();
    }

    internal async Task<IResult> Delete(IUserService service, Guid userId)
    {
        var user = await service.GetById(userId);
        if (user is null)
            return Results.NotFound();

        await service.Delete(user);
        return Results.Ok();
    }

    public void DefineServices(IServiceCollection services)
    {
        services.AddScoped<IUserService, UserService>();
    }
}