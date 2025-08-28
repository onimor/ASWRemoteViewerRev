using ASW.RemoteViewing.Shared.Dto.RemoteCar;
using ASW.Shared.Requests.RemoteCar;

namespace ASW.RemoteViewing.Features.RemoteCar.Services;

public interface IRemoteCarService
{
    Task<AddRemoteCarRequest> CreateAsync(AddRemoteCarRequest addRemoteCarRequest); 
    Task<RemoteCarDto?> GetByIdAsync(Guid carId);
    Task<List<RemoteCarDto>> GetAllAsync(); 
    Task UpdateAsync(UpdateRemoteCarRequest updateRemoteCarRequest); 
    Task DeleteAsync(Guid carId);

    // Дополнительные специфичные методы
    Task<RemoteCarDto?> GetByNumber(string carNumber);
}
