using ASW.RemoteViewing.Features.RemoteTrailer.Entities;
using ASW.RemoteViewing.Shared.Dto.RemoteTrailer;
using AutoMapper;

namespace ASW.RemoteViewing.Features.RemoteTrailer.Mapping;

public class RemoteTrailerMappingProfile : Profile
{
    public RemoteTrailerMappingProfile()
    {
        CreateMap<RemoteTrailerEF, RemoteTrailerDto>();
    }
}
