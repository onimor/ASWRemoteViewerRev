using ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;
using ASW.RemoteViewing.Features.RemoteGood.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.BaseServices;
using ASW.RemoteViewing.Shared.Dto.RemoteGood;
using ASW.Shared.Requests.RemoteGood;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Features.RemoteGood.Services;

public class RemoteGoodService : EntityRemoteBaseService<
    PgDbContext,
    RemoteGoodEF,
    RemoteGoodDto,
    AddRemoteGoodRequest,
    UpdateRemoteGoodRequest>,
    IRemoteGoodService
{
    public RemoteGoodService(
        PgDbContext context,
        ICurrentPlaceUserProvider placeUserProvider,
        IMapper mapper)
        : base(context, placeUserProvider, mapper)
    {
    }

    public async Task<RemoteGoodDto?> GetByIdAsync(Guid? goodId)
    {
        if (goodId == null)
            return null;

        return await _dbSet
            .Where(x => !x.IsRemoved && x.Id == goodId)
            .ProjectTo<RemoteGoodDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    }

    protected override Task<RemoteGoodEF?> FindEntityToUpdateAsync(UpdateRemoteGoodRequest request)
    {
        return _dbSet.FirstOrDefaultAsync(x => x.GoodId == request.Id && !x.IsRemoved);
    }

    protected override RemoteGoodEF MapFromCreateRequest(AddRemoteGoodRequest request)
    {
        return new RemoteGoodEF
        {
            GoodId = request.Id,
            Number = request.Number,
            Name = request.Name, 
            Unit = request.Unit,
            VendorCode = request.VendorCode,
        };
    }

    protected override RemoteGoodEF MapFromUpdateRequest(UpdateRemoteGoodRequest request)
    {
        return new RemoteGoodEF
        {
            GoodId = request.Id,
            Number = request.Number,
            Name = request.Name,
            Unit = request.Unit,
            VendorCode = request.VendorCode,
        };
    }

    protected override void MapToExistingEntity(RemoteGoodEF entity, UpdateRemoteGoodRequest request)
    {
        entity.Number = request.Number;
        entity.Name = request.Name;
        entity.Unit = request.Unit;
        entity.VendorCode = request.VendorCode;
    }
}