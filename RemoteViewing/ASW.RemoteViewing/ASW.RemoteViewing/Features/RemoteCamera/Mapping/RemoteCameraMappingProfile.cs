using ASW.RemoteViewing.Features.RemoteCamera.Entities;
using ASW.RemoteViewing.Shared.Dto.RemoteCamera;
using AutoMapper;

namespace ASW.RemoteViewing.Features.RemoteCamera.Mapping;

public class RemoteCameraMappingProfile : Profile
{
    public RemoteCameraMappingProfile()
    {
        // EF → DTO
        CreateMap<RemoteCameraEF, RemoteCameraDto>();
    }
}