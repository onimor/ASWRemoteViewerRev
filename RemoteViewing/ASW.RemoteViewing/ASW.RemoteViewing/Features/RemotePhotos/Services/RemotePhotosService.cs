using ASW.RemoteViewing.Features.RemotePhotos.Entities;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Shared.Dto.RemotePhoto;
using ASW.Shared.Requests.RemoteEmptyWeighingPhoto;
using ASW.Shared.Requests.RemotePhotoBrutto;
using ASW.Shared.Requests.RemotePhotoTara;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace ASW.RemoteViewing.Features.RemotePhotos.Services;

public class RemotePhotosService : IRemotePhotosService
{
    private readonly PgDbContext _context;
    protected readonly IMapper _mapper;

    public RemotePhotosService(PgDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task AddBruttoPhotoAsync(AddRemotePhotoBruttoRequest photoBrutto)
    {
        await _context.RemotePhotosBrutto.AddAsync(new RemotePhotoBruttoEF
        {
            WeighingId = photoBrutto.WeighingId,
            PhotoBruttoId = photoBrutto.Id,
            Date = photoBrutto.Date,
            Base64Image = photoBrutto.Base64Image
        });

        await _context.SaveChangesAsync();
    }

    public async Task DeleteBruttoPhotosAsync(Guid weighingId)
    {
        await _context.RemotePhotosBrutto
            .Where(x => x.WeighingId == weighingId)
            .ExecuteDeleteAsync();
    }

    public async Task AddTaraPhotoAsync(AddRemotePhotoTaraRequest photoTara)
    {
        await _context.RemotePhotosTara.AddAsync(new RemotePhotoTaraEF
        {
            WeighingId = photoTara.WeighingId,
            PhotoTaraId = photoTara.Id,
            Date = photoTara.Date,
            Base64Image = photoTara.Base64Image
        });

        await _context.SaveChangesAsync();
    }

    public async Task DeleteTaraPhotosAsync(Guid weighingId)
    {
        await _context.RemotePhotosTara
            .Where(x => x.WeighingId == weighingId)
            .ExecuteDeleteAsync();
    }

    public async Task AddEmptyWeighingPhotoAsync(AddRemoteEmptyWeighingPhotoRequest emptyWeighingPhoto)
    {
        await _context.RemoteEmptyWeighingPhotos.AddAsync(new RemoteEmptyWeighingPhotoEF
        {
            EmptyWeighingId = emptyWeighingPhoto.EmptyWeighingId,
            EmptyWeighingPhotoId = emptyWeighingPhoto.Id,
            Date = emptyWeighingPhoto.Date,
            Base64Image = emptyWeighingPhoto.Base64Image
        });

        await _context.SaveChangesAsync();
    }

    public async Task DeleteEmptyWeighingPhotosAsync(Guid emptyWeighingId)
    {
        await _context.RemoteEmptyWeighingPhotos
            .Where(x => x.EmptyWeighingId == emptyWeighingId)
            .ExecuteDeleteAsync();
    }

    public async Task<List<RemotePhotoBruttoDto>> GetBruttoPhotoAsync(Guid weighingId)
    {
        return await _context.RemotePhotosBrutto
            .Where(x => x.WeighingId == weighingId)
            .OrderBy(x => x.Date)
            .ProjectTo<RemotePhotoBruttoDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<List<RemotePhotoTaraDto>> GetTaraPhotoAsync(Guid weighingId)
    {
        return await _context.RemotePhotosTara
            .Where(x => x.WeighingId == weighingId)
            .OrderBy(x => x.Date)
            .ProjectTo<RemotePhotoTaraDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<List<RemoteEmptyWeighingPhotoDto>?> GetEmptyWeighingPhotoAsync(Guid emptyWeighingId)
    {
        return await _context.RemoteEmptyWeighingPhotos
            .Where(x => x.EmptyWeighingId == emptyWeighingId)
            .OrderBy(x => x.Date)
            .ProjectTo<RemoteEmptyWeighingPhotoDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}