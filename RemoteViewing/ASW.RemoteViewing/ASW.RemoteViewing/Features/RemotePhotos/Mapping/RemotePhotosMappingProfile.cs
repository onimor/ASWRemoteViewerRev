using ASW.RemoteViewing.Features.RemotePhotos.Entities;
using ASW.RemoteViewing.Shared.Dto.RemotePhoto;
using AutoMapper;

namespace ASW.RemoteViewing.Features.RemotePhotos.Mapping;
public class RemotePhotosMappingProfile : Profile
{
    public RemotePhotosMappingProfile()
    {
        CreateMap<RemotePhotoBruttoEF, RemotePhotoBruttoDto>();
        CreateMap<RemotePhotoTaraEF, RemotePhotoTaraDto>();
        CreateMap<RemoteEmptyWeighingPhotoEF, RemoteEmptyWeighingPhotoDto>();
    }
}
