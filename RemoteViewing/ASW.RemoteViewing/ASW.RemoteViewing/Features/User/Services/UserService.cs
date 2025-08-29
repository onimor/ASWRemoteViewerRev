using ASW.RemoteViewing.Api.Hub;
using ASW.RemoteViewing.Features.User.Entities;
using ASW.RemoteViewing.Features.User.Mapping;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.Constants;
using ASW.RemoteViewing.Shared.Dto.User;
using ASW.RemoteViewing.Shared.Requests.User;
using ASW.RemoteViewing.Shared.Security;
using ASW.Shared.Extension;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore; 

namespace ASW.RemoteViewing.Features.User.Services;

public class UserService : IUserService
{
    private readonly PgDbContext _pgDbContext;
    private readonly IHubContext<NotificationHub> _hub;

    public UserService(IHubContext<NotificationHub> hub, PgDbContext pgDbContext)
    {
        _hub = hub;
        _pgDbContext = pgDbContext;
    }

    public async Task Create(CreateUserRequest newUser)
    {
        var checkUser = await _pgDbContext.Users
            .FirstOrDefaultAsync(c => c.IsRemoved == false && c.Login == newUser.Login);

        if (checkUser is not null)
            throw new ValidationException($"Пользователь '{checkUser.Login}' уже существует");
        
        var role = Roles.GetRole(newUser.Role)
            ?? throw new ValidationException($"Роль не существует");

        var user = newUser.ToEntity();
        user.Role = role;
        user.IsRemoved = false;
        user.Password = PassHelper<UserEF>.GetHash(user, newUser.Password);

        await _pgDbContext.Users.AddAsync(user);
        await _pgDbContext.SaveChangesAsync();
    }

    public async Task Delete(UserEF user)
    {
        user.IsRemoved = true;
        _pgDbContext.Users.Update(user!);
        await _pgDbContext.SaveChangesAsync();
        await _hub.Clients.All.SendAsync(NotificationHubEvents.UsersChange + user.Id.ToString());
    }

    public async Task<UserEF?> GetById(Guid userId)
    {
        return await _pgDbContext.Users.FirstOrDefaultAsync(c =>
            c.IsRemoved == false && c.Id == userId);
    }

    public async Task<List<UserDto>> GetAll()
    {
        var users = await _pgDbContext.Users
            .Where(c => c.IsRemoved == false)
            .OrderBy(c => c.ReductionFIO)
            .ToListAsync();

        return users.Select(u => u.ToDto()).ToList();
    }

    public async Task Update(Guid userId, UpdateUserRequest updateUser)
    {
        var user = await _pgDbContext.Users
            .FirstOrDefaultAsync(u => u.Login == updateUser.Login && u.IsRemoved == false);

        if (user is not null && user.Id != userId)
            throw new ValidationException($"{updateUser?.Login} уже существует.");

        user = await _pgDbContext.Users
            .FirstOrDefaultAsync(u => u.Id == userId && u.IsRemoved == false);

        if (user is null)
            throw new ValidationException($"Пользователя не существует.");

        var role = Roles.GetRole(updateUser.Role)
           ?? throw new ValidationException($"Роль не существует");

        if (updateUser.IsNeedNewPassword)
            user.Password = PassHelper<UserEF>.GetHash(user, updateUser.Password);

        user.Login = updateUser.Login;
        user.Role = role;
        user.ReductionFIO = updateUser.FIO;
        user.ModifiedId = Guid.NewGuid();

        await _pgDbContext.SaveChangesAsync();
        await _hub.Clients.All.SendAsync(NotificationHubEvents.UsersChange + user.Id.ToString());
    }
}
