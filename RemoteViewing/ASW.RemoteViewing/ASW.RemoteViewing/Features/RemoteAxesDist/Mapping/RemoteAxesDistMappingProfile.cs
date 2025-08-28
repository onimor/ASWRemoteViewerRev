using ASW.RemoteViewing.Features.RemoteAxesDist.Entities;
using ASW.RemoteViewing.Shared.Dto.RemoteAxes;
using AutoMapper;

namespace ASW.RemoteViewing.Features.RemoteAxesDist.Mapping;

public class RemoteAxesDistMappingProfile : Profile
{
    public RemoteAxesDistMappingProfile()
    {
        CreateMap<RemoteAxesDistEF, RemoteAxesDistDto>();
    }
}
