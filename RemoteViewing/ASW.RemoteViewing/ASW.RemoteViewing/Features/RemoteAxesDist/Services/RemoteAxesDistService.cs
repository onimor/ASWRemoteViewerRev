using ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;
using ASW.RemoteViewing.Features.RemoteAxesDist.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.Dto.RemoteAxes;
using ASW.Shared.Requests.RemoteAxesDist;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity;

namespace ASW.RemoteViewing.Features.RemoteAxesDist.Services;

public class RemoteAxesDistService : IRemoteAxesDistService
{ 
    private readonly PgDbContext _context;
    private readonly ICurrentPlaceUserProvider _placeUserProvider;
    protected readonly IMapper _mapper;
    public RemoteAxesDistService(PgDbContext context, ICurrentPlaceUserProvider placeUserProvider, IMapper mapper)
    {
        _context = context;
        _placeUserProvider = placeUserProvider;
        _mapper = mapper;
    }
    public async Task<AddRemoteAxesDistRequest> CreateAsync(AddRemoteAxesDistRequest addRemoteAxesDistRequest)
    {
        var placeUser = await _placeUserProvider.GetCurrentUserAsync();
        _context.RemoteAxesDists.Add(new RemoteAxesDistEF
        {
            PlaceId = placeUser.Id,
            PlaceName = placeUser.Name,
            WeighingId = addRemoteAxesDistRequest.WeighingId,
            AxesDistId = addRemoteAxesDistRequest.Id,
            Number = addRemoteAxesDistRequest.Number,
            Tara = addRemoteAxesDistRequest.Tara,
            Brutto = addRemoteAxesDistRequest.Brutto,
        });
        await _context.SaveChangesAsync(); 
        return addRemoteAxesDistRequest;
    }
    public async Task DeleteAsync(Guid weighingId)
    {
        await _context.RemoteAxesDists.Where(x => x.WeighingId == weighingId).ExecuteDeleteAsync();
    }
    public async Task<List<RemoteAxesDistDto>?> GetByWeighingAsync(Guid weighingId)
    { 
        return await _context.RemoteAxesDists
            .Where(x => x.WeighingId == weighingId)
            .OrderBy(x => x.Number)
            .ProjectTo<RemoteAxesDistDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
