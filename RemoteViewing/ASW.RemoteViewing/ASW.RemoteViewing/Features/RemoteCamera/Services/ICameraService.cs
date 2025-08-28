using ASW.RemoteViewing.Shared.Dto.RemoteCamera;
using ASW.Shared.Requests.RemoteCamera;

namespace ASW.RemoteViewing.Features.RemoteCamera.Services;

public interface IRemoteCameraService
{
    Task<AddRemoteCameraRequest> CreateAsync(AddRemoteCameraRequest request);
    Task UpdateAsync(UpdateRemoteCameraRequest request);
    Task DeleteAsync(Guid cameraId);
    Task<List<RemoteCameraDto>> GetAllAsync();
    Task<RemoteCameraDto?> GetByIdAsync(Guid id);

    // Дополнительные специфичные методы
    Task<List<RemoteCameraDto>> GetAllByPostAsync(Guid postId);
    Task DeleteByPostAsync(Guid postId);  
}
