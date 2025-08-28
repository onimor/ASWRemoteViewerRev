using ASW.RemoteViewing.Shared.Dto.RemotePost;
using ASW.Shared.Requests.RemotePost;

namespace ASW.RemoteViewing.Features.RemotePost.Services;

public interface IRemotePostService
{
    Task<AddRemotePostRequest> CreateAsync(AddRemotePostRequest addRemotePostRequest); 
    Task<RemotePostDto?> GetByIdAsync(Guid id);
    Task<List<RemotePostDto>> GetAllAsync(); 
    Task UpdateAsync(UpdateRemotePostRequest updateRemotePostRequest); 
    Task DeleteAsync(Guid postId);
}
