using ASW.RemoteViewing.Features.RemoteCounterparty.Entities;
using ASW.RemoteViewing.Shared.Dto.RemoteCounterparty;
using AutoMapper;

namespace ASW.RemoteViewing.Features.RemoteCounterparty.Mapping;

public class RemoteCounterpartyMappingProfile : Profile
{
    public RemoteCounterpartyMappingProfile()
    {
        CreateMap<RemoteCounterpartyEF, RemoteCounterpartyDto>();
    }
}
