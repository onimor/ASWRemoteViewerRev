using ASW.RemoteViewing.Features.RemoteWeighing.Entities;
using ASW.RemoteViewing.Shared.Dto.RemoteWeighing;
using AutoMapper;

namespace ASW.RemoteViewing.Features.RemoteWeighing.Mapping;

public class RemoteWeighingMappingProfile : Profile
{
    public RemoteWeighingMappingProfile()
    {
        CreateMap<RemoteWeighingEF, RemoteWeighingDto>();
    }
}
