using ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;
using ASW.RemoteViewing.Features.RemoteDriver.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.BaseServices;
using ASW.RemoteViewing.Shared.Dto.RemoteDriver;
using ASW.Shared.Requests.RemoteDriver;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Features.RemoteDriver.Services;

public class RemoteDriverService : EntityRemoteBaseService<
    PgDbContext,
    RemoteDriverEF,
    RemoteDriverDto,
    AddRemoteDriverRequest,
    UpdateRemoteDriverRequest>,
    IRemoteDriverService
{
    public RemoteDriverService(
        PgDbContext context,
        ICurrentPlaceUserProvider placeUserProvider,
        IMapper mapper)
        : base(context, placeUserProvider, mapper)
    {
    }
    protected override Task<RemoteDriverEF?> FindEntityToUpdateAsync(UpdateRemoteDriverRequest request)
    {
        return _dbSet.FirstOrDefaultAsync(x => x.DriverId == request.Id && !x.IsRemoved);
    }
    protected override void MapToExistingEntity(RemoteDriverEF driver, UpdateRemoteDriverRequest request)
    {
        driver.Name = request.Name;
        driver.RegistrationNumber = request.RegistrationNumber; 
    }
    protected override RemoteDriverEF MapFromCreateRequest(AddRemoteDriverRequest request)
    { 
        return new RemoteDriverEF
        { 
            DriverId = request.Id,
            Name = request.Name,
            RegistrationNumber = request.RegistrationNumber, 
        };
    }
    protected override RemoteDriverEF MapFromUpdateRequest(UpdateRemoteDriverRequest request)
    { 
        return new RemoteDriverEF
        { 
            DriverId = request.Id,
            Name = request.Name,
            RegistrationNumber = request.RegistrationNumber,
        };
    }
}
