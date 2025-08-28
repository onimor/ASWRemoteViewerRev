using ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;
using ASW.RemoteViewing.Features.RemoteTrailer.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.BaseServices;
using ASW.RemoteViewing.Shared.Dto.RemoteTrailer;
using ASW.Shared.Requests.RemoteTrailer;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Features.RemoteTrailer.Services;

public class RemoteTrailerService : EntityRemoteBaseService<
    PgDbContext,
    RemoteTrailerEF,
    RemoteTrailerDto,
    AddRemoteTrailerRequest,
    UpdateRemoteTrailerRequest>,
    IRemoteTrailerService
{
    public RemoteTrailerService(
        PgDbContext context,
        ICurrentPlaceUserProvider placeUserProvider,
        IMapper mapper)
        : base(context, placeUserProvider, mapper)
    {
    }

    protected override Task<RemoteTrailerEF?> FindEntityToUpdateAsync(UpdateRemoteTrailerRequest request)
    {
        return _dbSet.FirstOrDefaultAsync(x => x.TrailerId == request.Id && !x.IsRemoved);
    }

    protected override void MapToExistingEntity(RemoteTrailerEF entity, UpdateRemoteTrailerRequest request)
    {
        entity.GovernmentNumber = request.GovernmentNumber;
        entity.Model = request.Model;
        entity.Brand = request.Brand;
        entity.Type = request.Type;
    }

    protected override RemoteTrailerEF MapFromCreateRequest(AddRemoteTrailerRequest request)
    {
        return new RemoteTrailerEF
        {
            TrailerId = request.Id,
            GovernmentNumber = request.GovernmentNumber,
            Model = request.Model,
            Brand = request.Brand,
            Type = request.Type,
        };
    }

    protected override RemoteTrailerEF MapFromUpdateRequest(UpdateRemoteTrailerRequest request)
    {
        return new RemoteTrailerEF
        {
            TrailerId = request.Id,
            GovernmentNumber = request.GovernmentNumber,
            Model = request.Model,
            Brand = request.Brand,
            Type = request.Type,
        };
    }
}