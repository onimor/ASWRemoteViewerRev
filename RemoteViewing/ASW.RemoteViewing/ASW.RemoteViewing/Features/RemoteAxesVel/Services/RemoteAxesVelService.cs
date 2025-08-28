using ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;
using ASW.RemoteViewing.Features.RemoteAxesVel.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.Dto.RemoteAxes;
using ASW.Shared.Requests.RemoteAxesVel;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace ASW.RemoteViewing.Features.RemoteAxesVel.Services;

public class RemoteAxesVelService:IRemoteAxesVelService
{
    private readonly PgDbContext _context;
    private readonly ICurrentPlaceUserProvider _placeUserProvider;
    protected readonly IMapper _mapper;
    public RemoteAxesVelService(PgDbContext context, ICurrentPlaceUserProvider placeUserProvider, IMapper mapper)
    { 
        _context = context;
        _placeUserProvider = placeUserProvider;
        _mapper = mapper;
    }
    public async Task CreateAsync(AddRemoteAxesVelRequest addRemoteAxesVelRequest)
    { 
        var placeUser = await _placeUserProvider.GetCurrentUserAsync();
        _context.RemoteAxesVels.Add(new RemoteAxesVelEF
        {
            PlaceId = placeUser.Id,
            PlaceName = placeUser.Name,
            WeighingId = addRemoteAxesVelRequest.WeighingId,
            AxesVelId = addRemoteAxesVelRequest.Id,
            Number = addRemoteAxesVelRequest.Number,
            Tara = addRemoteAxesVelRequest.Tara,
            Brutto = addRemoteAxesVelRequest.Brutto
        });
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid weighingId)
    {
        await _context.RemoteAxesVels.Where(x => x.WeighingId == weighingId).ExecuteDeleteAsync();
    }

    public async Task<List<RemoteAxesVelDto>?> GetByWeighingAsync(Guid weighingId)
    {
        
        return await _context.RemoteAxesVels
            .Where(x => x.WeighingId == weighingId)
            .OrderBy(x => x.Number)
            .ProjectTo<RemoteAxesVelDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
