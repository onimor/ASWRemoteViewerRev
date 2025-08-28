using ASW.RemoteViewing.Features.RemoteAxesWeight.Entities;
using ASW.RemoteViewing.Shared.Dto.RemoteAxes;
using AutoMapper;

namespace ASW.RemoteViewing.Features.RemoteAxesWeight.Mapping;

public class RemoteAxesWeightMappingProfile : Profile
{
    public RemoteAxesWeightMappingProfile()
    {
        CreateMap<RemoteAxesWeightEF, RemoteAxesWeightDto>();
    }
}
