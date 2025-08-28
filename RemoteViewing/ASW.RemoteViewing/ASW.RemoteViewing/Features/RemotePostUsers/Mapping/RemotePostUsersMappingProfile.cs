using ASW.RemoteViewing.Features.RemotePostUsers.Entities;
using ASW.RemoteViewing.Shared.Dto.RemotePostUsers;
using AutoMapper;

namespace ASW.RemoteViewing.Features.RemotePostUsers.Mapping;

public class RemotePostUsersMappingProfile : Profile
{
    public RemotePostUsersMappingProfile()
    {
        CreateMap<RemotePostUserEF, RemotePostUserDto>();
    }
}