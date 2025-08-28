using ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;
using ASW.RemoteViewing.Features.RemotePost.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.BaseServices;
using ASW.RemoteViewing.Shared.Dto.RemotePost;
using ASW.Shared.Requests.RemotePost;
using AutoMapper;
using Microsoft.EntityFrameworkCore;


namespace ASW.RemoteViewing.Features.RemotePost.Services;

public class RemotePostService : EntityRemoteBaseService<
    PgDbContext,
    RemotePostEF,
    RemotePostDto,
    AddRemotePostRequest,
    UpdateRemotePostRequest>,
    IRemotePostService
{
    public RemotePostService(
        PgDbContext context,
        ICurrentPlaceUserProvider placeUserProvider,
        IMapper mapper)
        : base(context, placeUserProvider, mapper)
    {
    }
    protected override Task<RemotePostEF?> FindEntityToUpdateAsync(UpdateRemotePostRequest request)
    {
        return _dbSet.FirstOrDefaultAsync(x => x.PostId == request.Id && !x.IsRemoved);
    }

    protected override RemotePostEF MapFromCreateRequest(AddRemotePostRequest request)
    {
        return new RemotePostEF
        {
            PostId = request.Id,
            Name = request.Name,
            ClientIP = request.ClientIP,    
            ClientPort = request.ClientPort,
            ComName = request.ComName,
            ComSpeed = request.ComSpeed,
            TCPIP = request.TCPIP,
            TCPPort = request.TCPPort,
            DelayEmptyScales = request.DelayEmptyScales,
            IsAutomaticDeviceModeOff = request.IsAutomaticDeviceModeOff,
            IsAutomaticMode = request.IsAutomaticMode,
            IsOnlyAxes = request.IsOnlyAxes,
            IsRegisteringEmptyPassage = request.IsRegisteringEmptyPassage,
            IsTcp = request.IsTcp,
            IsWeighingByAxes = request.IsWeighingByAxes,
            OperatingMode = request.OperatingMode,
            ServerIP = request.ServerIP,
            ServerPort = request.ServerPort,
            TerminalNumber = request.TerminalNumber,
            ThresholdBeginningWeighing = request.ThresholdBeginningWeighing, 
        };
    }

    protected override RemotePostEF MapFromUpdateRequest(UpdateRemotePostRequest request)
    {
        return new RemotePostEF
        {
            PostId = request.Id,
            Name = request.Name,
            ClientIP = request.ClientIP,
            ClientPort = request.ClientPort,
            ComName = request.ComName,
            ComSpeed = request.ComSpeed,
            TCPIP = request.TCPIP,
            TCPPort = request.TCPPort,
            DelayEmptyScales = request.DelayEmptyScales,
            IsAutomaticDeviceModeOff = request.IsAutomaticDeviceModeOff,
            IsAutomaticMode = request.IsAutomaticMode,
            IsOnlyAxes = request.IsOnlyAxes,
            IsRegisteringEmptyPassage = request.IsRegisteringEmptyPassage,
            IsTcp = request.IsTcp,
            IsWeighingByAxes = request.IsWeighingByAxes,
            OperatingMode = request.OperatingMode,
            ServerIP = request.ServerIP,
            ServerPort = request.ServerPort,
            TerminalNumber = request.TerminalNumber,
            ThresholdBeginningWeighing = request.ThresholdBeginningWeighing,
        };
    } 

    protected override void MapToExistingEntity(RemotePostEF entity, UpdateRemotePostRequest request)
    {
        entity.Name = request.Name;
        entity.ClientIP = request.ClientIP;
        entity.ClientPort = request.ClientPort;
        entity.ComName = request.ComName;
        entity.ComSpeed = request.ComSpeed;
        entity.TCPIP = request.TCPIP;
        entity.TCPPort = request.TCPPort;
        entity.DelayEmptyScales = request.DelayEmptyScales;
        entity.IsAutomaticDeviceModeOff = request.IsAutomaticDeviceModeOff;
        entity.IsAutomaticMode = request.IsAutomaticMode;
        entity.IsOnlyAxes = request.IsOnlyAxes;
        entity.IsRegisteringEmptyPassage = request.IsRegisteringEmptyPassage;
        entity.IsTcp = request.IsTcp;
        entity.IsWeighingByAxes = request.IsWeighingByAxes;
        entity.OperatingMode = request.OperatingMode;
        entity.ServerIP = request.ServerIP;
        entity.ServerPort = request.ServerPort;
        entity.TerminalNumber = request.TerminalNumber;
        entity.ThresholdBeginningWeighing = request.ThresholdBeginningWeighing;
    }
}
