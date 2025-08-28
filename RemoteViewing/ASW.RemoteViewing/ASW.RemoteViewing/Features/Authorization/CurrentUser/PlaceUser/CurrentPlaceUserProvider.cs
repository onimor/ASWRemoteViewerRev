using ASW.RemoteViewing.Features.Authorization.UserContext;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.Dto.PlaceUser;
using ASW.Shared.Extentions;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;

public class CurrentPlaceUserProvider : ICurrentPlaceUserProvider
{
    private readonly IUserContext _userContext;
    private readonly PgDbContext _context;

    public CurrentPlaceUserProvider(IUserContext userContext, PgDbContext context)
    {
        _userContext = userContext;
        _context = context;
    }

    public async Task<PlaceUserDto> GetCurrentUserAsync()
    {
        var id = _userContext.UserId;
        var user = await _context.PlaceUsers
            .FirstOrDefaultAsync(x => x.Id == _userContext.UserId && x.IsRemoved == false)
            ?? throw new ValidationException("Нет доступа");

        return new PlaceUserDto 
        { 
            Id = user.Id, 
            Name = user.Name, 
            ModifiedId = user.ModifiedId,
        };
    }
}