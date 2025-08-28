using ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;
using ASW.RemoteViewing.Features.RemoteWeighing.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.BaseServices;
using ASW.RemoteViewing.Shared.Dto.RemoteWeighing;
using ASW.RemoteViewing.Shared.Requests.RemoteWeighing;
using ASW.Shared.Requests.RemoteWeighing;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Features.RemoteWeighing.Services;

public class RemoteWeighingService : EntityRemoteBaseService<
    PgDbContext,
    RemoteWeighingEF,
    RemoteWeighingDto,
    AddRemoteWeighingRequest,
    UpdateRemoteWeighingRequest>,
    IRemoteWeighingService
{
    public RemoteWeighingService(
        PgDbContext context,
        ICurrentPlaceUserProvider placeUserProvider,
        IMapper mapper)
        : base(context, placeUserProvider, mapper)
    {
    }
   
    public async Task<List<RemoteWeighingDto>> GetLastOneWeighingAsync(Guid postId)
    {
        var weightingsDto = await _context.RemoteWeightings
            .Where(x => x.PostId == postId && x.IsRemoved == false && x.IsFormed == false && x.DateCreation <= DateTime.Now && x.DateCreation >= DateTime.Now.AddHours(-72))
            .ProjectTo<RemoteWeighingDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return weightingsDto;
    }
    public async Task<List<RemoteWeighingDto>> GetByDateAsync(RemoteWeightByDateRequest remoteWeightByDateRequest)
    {
        var weightingsDto = await _context.RemoteWeightings
                .Where(x => x.DateCreation >= remoteWeightByDateRequest.DateStart
                && x.DateCreation <= remoteWeightByDateRequest.DateEnd)
                .OrderBy(x => x.Number)
                .ProjectTo<RemoteWeighingDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
        return weightingsDto;
    }
    public async Task<List<RemoteWeighingDto>> GetByPostAndDateAsync(RemoteWeightByDateRequest remoteWeightByDateRequest, Guid postId)
    {
        return await _context.RemoteWeightings
                .Where(x => x.DateCreation >= remoteWeightByDateRequest.DateStart
                && x.DateCreation <= remoteWeightByDateRequest.DateEnd
                && x.PostId == postId)
                .OrderBy(x => x.Number)
                 .ProjectTo<RemoteWeighingDto>(_mapper.ConfigurationProvider)
                .ToListAsync();
    }
    protected override Task<RemoteWeighingEF?> FindEntityToUpdateAsync(UpdateRemoteWeighingRequest request)
    {
        return _dbSet.FirstOrDefaultAsync(x => x.WeighingId == request.Id && !x.IsRemoved);
    }

    protected override void MapToExistingEntity(RemoteWeighingEF entity, UpdateRemoteWeighingRequest request)
    {
        entity.Number = request.Number;
        entity.PostId = request.PostId;
        entity.AddedUserId = request.AddedUserId;
        entity.AddedUserName = request.AddedUserName;
        entity.DateCreation = request.DateCreation;
        entity.DateTara = request.DateTara;
        entity.DateBrutto = request.DateBrutto;
        entity.DateRegistration = request.DateRegistration;
        entity.Tara = request.Tara;
        entity.Brutto = request.Brutto;
        entity.Netto = request.Netto;
        entity.CarId = request.CarId;
        entity.CarName = request.CarName;
        entity.DriverId = request.DriverId;
        entity.DriverName = request.DriverName;
        entity.GoodsId = request.GoodsId;
        entity.GoodsName = request.GoodsName;
        entity.TrailerId = request.TrailerId;
        entity.TrailerName = request.TrailerName;
        entity.SenderId = request.SenderId;
        entity.SenderName = request.SenderName;
        entity.RecipientId = request.RecipientId;
        entity.RecipientName = request.RecipientName;
        entity.CarrierId = request.CarrierId;
        entity.CarrierName = request.CarrierName;
        entity.PayerId = request.PayerId;
        entity.PayerName = request.PayerName;
        entity.RFID = request.RFID;
        entity.DeliveryMethod = request.DeliveryMethod;
        entity.IsFormed = request.IsFormed;
        entity.IsEmptyWeighing = request.IsEmptyWeighing;
        entity.IsBloc = request.IsBloc;
        entity.IsRemoved = request.IsRemoved;
    }

    protected override RemoteWeighingEF MapFromCreateRequest(AddRemoteWeighingRequest request)
    {
        return new RemoteWeighingEF
        {
            WeighingId = request.Id,
            Number = request.Number,
            PostId = request.PostId,
            AddedUserId = request.AddedUserId,
            AddedUserName = request.AddedUserName,
            DateCreation = request.DateCreation,
            DateTara = request.DateTara,
            DateBrutto = request.DateBrutto,
            DateRegistration = request.DateRegistration,
            Tara = request.Tara,
            Brutto = request.Brutto,
            Netto = request.Netto,
            CarId = request.CarId,
            CarName = request.CarName,
            DriverId = request.DriverId,
            DriverName = request.DriverName,
            GoodsId = request.GoodsId,
            GoodsName = request.GoodsName,
            TrailerId = request.TrailerId,
            TrailerName = request.TrailerName,
            SenderId = request.SenderId,
            SenderName = request.SenderName,
            RecipientId = request.RecipientId,
            RecipientName = request.RecipientName,
            CarrierId = request.CarrierId,
            CarrierName = request.CarrierName,
            PayerId = request.PayerId,
            PayerName = request.PayerName,
            RFID = request.RFID,
            DeliveryMethod = request.DeliveryMethod,
            IsFormed = request.IsFormed,
            IsEmptyWeighing = request.IsEmptyWeighing,
            IsBloc = request.IsBloc,
            IsRemoved = request.IsRemoved,
        };
    }

    protected override RemoteWeighingEF MapFromUpdateRequest(UpdateRemoteWeighingRequest request)
    {
        return new RemoteWeighingEF
        {
            WeighingId = request.Id,
            Number = request.Number,
            PostId = request.PostId,
            AddedUserId = request.AddedUserId,
            AddedUserName = request.AddedUserName,
            DateCreation = request.DateCreation,
            DateTara = request.DateTara,
            DateBrutto = request.DateBrutto,
            DateRegistration = request.DateRegistration,
            Tara = request.Tara,
            Brutto = request.Brutto,
            Netto = request.Netto,
            CarId = request.CarId,
            CarName = request.CarName,
            DriverId = request.DriverId,
            DriverName = request.DriverName,
            GoodsId = request.GoodsId,
            GoodsName = request.GoodsName,
            TrailerId = request.TrailerId,
            TrailerName = request.TrailerName,
            SenderId = request.SenderId,
            SenderName = request.SenderName,
            RecipientId = request.RecipientId,
            RecipientName = request.RecipientName,
            CarrierId = request.CarrierId,
            CarrierName = request.CarrierName,
            PayerId = request.PayerId,
            PayerName = request.PayerName,
            RFID = request.RFID,
            DeliveryMethod = request.DeliveryMethod,
            IsFormed = request.IsFormed,
            IsEmptyWeighing = request.IsEmptyWeighing,
            IsBloc = request.IsBloc,
            IsRemoved = request.IsRemoved,
        };
    }
}
