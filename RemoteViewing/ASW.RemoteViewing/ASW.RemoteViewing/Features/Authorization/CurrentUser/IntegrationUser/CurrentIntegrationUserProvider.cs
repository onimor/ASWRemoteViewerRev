using ASW.RemoteViewing.Features.Authorization.UserContext;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.Dto.IntegrationUser;
using ASW.Shared.Extension;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Features.Authorization.CurrentUser.IntegrationUser;

public class CurrentIntegrationUserProvider : ICurrentIntegrationUserProvider
{
    private readonly IUserContext _userContext;
    private readonly PgDbContext _context;

    public CurrentIntegrationUserProvider(IUserContext userContext, PgDbContext context)
    {
        _userContext = userContext;
        _context = context;
    }

    public async Task<IntegrationUserDto> GetCurrentUserAsync()
    {
        var user = await _context.IntegrationUsers
            .FirstOrDefaultAsync(x => x.Id == _userContext.UserId && x.IsRemoved == false)
            ?? throw new ValidationException("Нет доступа");

        return new IntegrationUserDto
        { 
            Id = user.Id,
            Name = user.Name,
            Role = user.Role,
            KeyHash = user.KeyHash,
            KeyPrefix = user.KeyPrefix
        };
    }
}