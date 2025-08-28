using ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;
using ASW.RemoteViewing.Features.RemoteEmptyWeighing.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.BaseServices;
using ASW.RemoteViewing.Shared.Dto.RemoteEmptyWeighing;
using ASW.RemoteViewing.Shared.Requests.RemoteWeighing;
using ASW.Shared.Requests.RemoteEmptyWeighing;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Features.RemoteEmptyWeighing.Services;

public class RemoteEmptyWeighingService : EntityRemoteBaseService<
    PgDbContext,
    RemoteEmptyWeighingEF,
    RemoteEmptyWeighingDto,
    AddRemoteEmptyWeighingRequest,
    UpdateRemoteEmptyWeighingRequest>,
    IRemoteEmptyWeighingService
{
    public RemoteEmptyWeighingService(
        PgDbContext context,
        ICurrentPlaceUserProvider placeUserProvider,
        IMapper mapper)
        : base(context, placeUserProvider, mapper)
    {
    }

    public async Task<List<RemoteEmptyWeighingDto>> GetByDateAsync(RemoteWeightByDateRequest request)
    {
        var emptyWeightings = await _context.RemoteEmptyWeightings
            .Where(x => x.IsRemoved == false
                && x.Date >= request.DateStart
                && x.Date <= request.DateEnd)
            .OrderBy(x => x.Date)
            .ProjectTo<RemoteEmptyWeighingDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return emptyWeightings;
    }
    public async Task<List<RemoteEmptyWeighingDto>> GetByPostAndDateAsync(RemoteWeightByDateRequest request, Guid postId)
    { 
        var emptyWeightings = await _context.RemoteEmptyWeightings
            .Where(x => x.IsRemoved == false
                && x.Date >= request.DateStart
                && x.Date <= request.DateEnd 
                && x.PostId == postId)
            .OrderBy(x => x.Date)
            .ProjectTo<RemoteEmptyWeighingDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return emptyWeightings;
    }
    protected override Task<RemoteEmptyWeighingEF?> FindEntityToUpdateAsync(UpdateRemoteEmptyWeighingRequest request)
    {
        return _dbSet.FirstOrDefaultAsync(x => x.EmptyWeighingId == request.Id && !x.IsRemoved);
    }

    protected override void MapToExistingEntity(RemoteEmptyWeighingEF emptyWeighing, UpdateRemoteEmptyWeighingRequest request)
    {
        emptyWeighing.PostId = request.PostId;
        emptyWeighing.PostName = request.PostName;
        emptyWeighing.Date = request.Date;
        emptyWeighing.CarName = request.CarName;
        emptyWeighing.TrailerName = request.TrailerName;
        emptyWeighing.Weight = request.Weight;
        emptyWeighing.Stability = request.Stability;
    }
    protected override RemoteEmptyWeighingEF MapFromCreateRequest(AddRemoteEmptyWeighingRequest request)
    { 
        return new RemoteEmptyWeighingEF
        { 
            EmptyWeighingId = request.Id,
            PostId = request.PostId,
            PostName = request.PostName,
            Date = request.Date,
            CarName = request.CarName,
            TrailerName = request.TrailerName,
            Weight = request.Weight, 
            Stability = request.Stability,
        };
    }
    protected override RemoteEmptyWeighingEF MapFromUpdateRequest(UpdateRemoteEmptyWeighingRequest request)
    { 
        return new RemoteEmptyWeighingEF
        {
            EmptyWeighingId = request.Id,
            PostId = request.PostId,
            PostName = request.PostName,
            Date = request.Date,
            CarName = request.CarName,
            TrailerName = request.TrailerName,
            Weight = request.Weight,
            Stability = request.Stability,
        };
    }
}
