using ASW.RemoteViewing.Features.RemotePost.Entities;
using ASW.RemoteViewing.Shared.Dto.RemotePost;
using AutoMapper;

namespace ASW.RemoteViewing.Features.RemotePost.Mapping;

public class RemotePostMappingProfile : Profile
{
    public RemotePostMappingProfile()
    {
        CreateMap<RemotePostEF, RemotePostDto>();
    }
}
