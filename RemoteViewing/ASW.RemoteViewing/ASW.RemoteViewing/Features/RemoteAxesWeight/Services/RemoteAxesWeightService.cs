using ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;
using ASW.RemoteViewing.Features.RemoteAxesWeight.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.Dto.RemoteAxes;
using ASW.Shared.Requests.RemoteAxesWeight;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Features.RemoteAxesWeight.Services;

public class RemoteAxesWeightService : IRemoteAxesWeightService
{ 
    private readonly PgDbContext _context;
    private readonly ICurrentPlaceUserProvider _placeUserProvider;
    protected readonly IMapper _mapper;

    public RemoteAxesWeightService(PgDbContext context, ICurrentPlaceUserProvider placeUserProvider, IMapper mapper)
    { 
        _context = context;
        _placeUserProvider = placeUserProvider;
        _mapper = mapper;
    }
    public async Task CreateAsync(AddRemoteAxesWeightRequest addRemoteAxesWeightRequest)
    {
        var placeUser = await _placeUserProvider.GetCurrentUserAsync();
        _context.RemoteAxesWeights.Add(new RemoteAxesWeightEF
        {
            PlaceId = placeUser.Id,
            PlaceName = placeUser.Name,
            WeighingId = addRemoteAxesWeightRequest.WeighingId,
            AxesWeightId = addRemoteAxesWeightRequest.Id,
            Number = addRemoteAxesWeightRequest.Number,
            Tara = addRemoteAxesWeightRequest.Tara,
            Brutto = addRemoteAxesWeightRequest.Brutto,
        });
        await _context.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid weighingId)
    {
        await _context.RemoteAxesWeights.Where(x => x.WeighingId == weighingId).ExecuteDeleteAsync();
    }
    public async Task<List<RemoteAxesWeightDto>?> GetByWeighingAsync(Guid weighingId)
    { 
        return await _context.RemoteAxesWeights
            .Where(x => x.WeighingId == weighingId)
            .OrderBy(x => x.Number)
            .ProjectTo<RemoteAxesWeightDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}
