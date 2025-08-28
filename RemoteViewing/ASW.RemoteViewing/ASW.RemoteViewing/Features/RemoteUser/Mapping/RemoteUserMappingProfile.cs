using ASW.RemoteViewing.Features.RemoteUser.Entities;
using ASW.RemoteViewing.Shared.Dto.RemoteUser;
using AutoMapper;

namespace ASW.RemoteViewing.Features.RemoteUser.Mapping;

public class RemoteUserMappingProfile : Profile
{
    public RemoteUserMappingProfile()
    {
        CreateMap<RemoteUserEF, RemoteUserDto>();
    }
}
