using ASW.RemoteViewing.Features.Authorization.CurrentUser.Default;
using ASW.RemoteViewing.Features.Authorization.UserContext;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.Dto.User;
using ASW.Shared.Extentions;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Features.CurrentUser.Default;

public class CurrentUserProvider : ICurrentUserProvider
{
    private readonly IUserContext _userContext;
    private readonly PgDbContext _context;

    public CurrentUserProvider(IUserContext userContext, PgDbContext context)
    {
        _userContext = userContext;
        _context = context;
    }

    public async Task<UserInfoDto> GetCurrentUserAsync()
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == _userContext.UserId && x.IsRemoved == false)
            ?? throw new ValidationException("Нет доступа");

        return new UserInfoDto 
        { 
            Id = user.Id,
            Login = user.Login,
            FullFIO = user.FullFIO,
            ReductionFIO = user.ReductionFIO,
            Role = user.Role,
            ModifiedId = user.ModifiedId,
        };
    }
}