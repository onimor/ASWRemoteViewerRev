using ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;
using ASW.RemoteViewing.Features.RemoteCamera.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.BaseServices;
using ASW.RemoteViewing.Shared.Dto.RemoteCamera;
using ASW.Shared.Requests.RemoteCamera;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Features.RemoteCamera.Services;

public class RemoteCameraService : EntityRemoteBaseService<
    PgDbContext,
    RemoteCameraEF,
    RemoteCameraDto,
    AddRemoteCameraRequest,
    UpdateRemoteCameraRequest>,
    IRemoteCameraService
{
    public RemoteCameraService(
        PgDbContext context,
        ICurrentPlaceUserProvider placeUserProvider,
        IMapper mapper)
        : base(context, placeUserProvider, mapper)
    {
    }

    public async Task DeleteByPostAsync(Guid postId)
    {
        await _context.RemoteCameras
            .Where(x => x.PostId == postId)
            .ExecuteDeleteAsync();
    }

    public async Task<List<RemoteCameraDto>> GetAllByPostAsync(Guid postId)
    {
        return await _context.RemoteCameras
            .Where(x => !x.IsRemoved && x.PostId == postId)
            .OrderBy(x => x.SequenceNumber)
            .ProjectTo<RemoteCameraDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    protected override Task<RemoteCameraEF?> FindEntityToUpdateAsync(UpdateRemoteCameraRequest request)
    {
        return _dbSet.FirstOrDefaultAsync(c => c.CameraId == request.Id && !c.IsRemoved);
    }

    protected override RemoteCameraEF MapFromCreateRequest(AddRemoteCameraRequest request)
    {
        return new RemoteCameraEF
        {
            CameraId = request.Id,
            Name = request.Name,
            PostId = request.PostId,
            SequenceNumber = request.SequenceNumber,
            IsRecognize = request.IsRecognize,
            X = request.X,
            Y = request.Y,
            Z = request.Z
        };
    }

    protected override RemoteCameraEF MapFromUpdateRequest(UpdateRemoteCameraRequest request)
    {
        return new RemoteCameraEF
        {
            CameraId = request.Id,
            Name = request.Name,
            PostId = request.PostId,
            SequenceNumber = request.SequenceNumber,
            IsRecognize = request.IsRecognize,
            X = request.X,
            Y = request.Y,
            Z = request.Z
        };
    }

    protected override void MapToExistingEntity(RemoteCameraEF entity, UpdateRemoteCameraRequest request)
    { 
        entity.Name = request.Name;
        entity.PostId = request.PostId;
        entity.SequenceNumber = request.SequenceNumber;
        entity.IsRecognize = request.IsRecognize;
        entity.X = request.X;
        entity.Y = request.Y;
        entity.Z = request.Z;
    }
}