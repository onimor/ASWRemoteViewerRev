using ASW.RemoteViewing.Features.RemoteEmptyWeighing.Entities;
using ASW.RemoteViewing.Shared.Dto.RemoteEmptyWeighing;
using AutoMapper;

namespace ASW.RemoteViewing.Features.RemoteEmptyWeighing.Mapping;

public class RemoteEmptyWeighingMappingProfile : Profile
{
    public RemoteEmptyWeighingMappingProfile()
    {
        CreateMap<RemoteEmptyWeighingEF, RemoteEmptyWeighingDto>();
    }
}
