using ASW.RemoteViewing.Features.RemoteCar.Entities;
using ASW.RemoteViewing.Shared.Dto.RemoteCar;
using AutoMapper;

namespace ASW.RemoteViewing.Features.RemoteCar.Mapping;

public class RemoteCarMappingProfile : Profile
{
    public RemoteCarMappingProfile()
    {
        CreateMap<RemoteCarEF, RemoteCarDto>();
    }
}
