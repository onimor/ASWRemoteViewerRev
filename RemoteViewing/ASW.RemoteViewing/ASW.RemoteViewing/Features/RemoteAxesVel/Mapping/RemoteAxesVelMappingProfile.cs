using ASW.RemoteViewing.Features.RemoteAxesVel.Entities;
using ASW.RemoteViewing.Shared.Dto.RemoteAxes;
using AutoMapper;

namespace ASW.RemoteViewing.Features.RemoteAxesVel.Mapping;

public class RemoteAxesVelMappingProfile : Profile
{
    public RemoteAxesVelMappingProfile()
    {
        CreateMap<RemoteAxesVelEF, RemoteAxesVelDto>();
    }
}
