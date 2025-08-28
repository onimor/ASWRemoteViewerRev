using ASW.RemoteViewing.Features.RemoteDriver.Entities;
using ASW.RemoteViewing.Shared.Dto.RemoteDriver;
using AutoMapper;

namespace ASW.RemoteViewing.Features.RemoteDriver.Mapping;

public class RemoteDriverMappingProfile : Profile
{
    public RemoteDriverMappingProfile()
    {
        CreateMap<RemoteDriverEF, RemoteDriverDto>();
    }
}