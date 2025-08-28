using ASW.RemoteViewing.Features.RemoteGood.Entities;
using ASW.RemoteViewing.Shared.Dto.RemoteGood;
using AutoMapper;

namespace ASW.RemoteViewing.Features.RemoteGood.Mapping;

public class RemoteGoodMappingProfile : Profile
{
    public RemoteGoodMappingProfile()
    {
        CreateMap<RemoteGoodEF, RemoteGoodDto>();
    }
}