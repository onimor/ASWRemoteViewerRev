using ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;
using ASW.RemoteViewing.Features.RemoteCar.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.BaseServices;
using ASW.RemoteViewing.Shared.Dto.RemoteCar;
using ASW.Shared.Requests.RemoteCar;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Features.RemoteCar.Services;

public class RemoteCarService : EntityRemoteBaseService<
    PgDbContext,
    RemoteCarEF,
    RemoteCarDto,
    AddRemoteCarRequest,
    UpdateRemoteCarRequest>,
    IRemoteCarService
{
    public RemoteCarService(
        PgDbContext context,
        ICurrentPlaceUserProvider placeUserProvider,
        IMapper mapper)
        : base(context, placeUserProvider, mapper)
    {
    }

    public async Task<RemoteCarDto?> GetByNumber(string carNumber)
    {
        return await _dbSet
            .Where(x => !x.IsRemoved && x.GovernmentNumber == carNumber)
            .ProjectTo<RemoteCarDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
    } 

    protected override Task<RemoteCarEF?> FindEntityToUpdateAsync(UpdateRemoteCarRequest request)
    {
        return _dbSet.FirstOrDefaultAsync(x => x.Id == request.Id && !x.IsRemoved);
    }

    protected override void MapToExistingEntity(RemoteCarEF entity, UpdateRemoteCarRequest request)
    {
        entity.GovernmentNumber = request.GovernmentNumber;
        entity.Brand = request.Brand;
        entity.Model = request.Model;
        entity.Type = request.Type;
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
    }
    protected override RemoteCarEF MapFromCreateRequest(AddRemoteCarRequest request)
    { 
        return new RemoteCarEF
        { 
            CarId = request.Id,
            GovernmentNumber = request.GovernmentNumber,
            Brand = request.Brand,
            Model = request.Model,
            Type = request.Type,
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
            RFID = request.RFID
        }; 
    }
    protected override RemoteCarEF MapFromUpdateRequest(UpdateRemoteCarRequest request)
    { 
        return new RemoteCarEF
        { 
            CarId = request.Id,
            GovernmentNumber = request.GovernmentNumber,
            Brand = request.Brand,
            Model = request.Model,
            Type = request.Type,
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
            RFID = request.RFID
        };
    }
}
