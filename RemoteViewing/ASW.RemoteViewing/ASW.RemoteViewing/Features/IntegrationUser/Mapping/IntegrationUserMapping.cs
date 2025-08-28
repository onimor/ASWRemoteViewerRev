using ASW.RemoteViewing.Features.IntegrationUser.Entities;
using ASW.RemoteViewing.Shared.Dto.IntegrationUser;
using ASW.RemoteViewing.Shared.Requests.IntegrationUser;

namespace ASW.RemoteViewing.Features.IntegrationUser.Mappings
{
    public static class IntegrationUserMapping
    {
        public static IntegrationUserDto ToDto(this IntegrationUserEF entity)
        {
            return new IntegrationUserDto
            {
                Id = entity.Id,
                Name = entity.Name,
                KeyPrefix = entity.KeyPrefix,
                KeyHash = entity.KeyHash,
                Role = entity.Role
            };
        }

        public static IntegrationUserEF ToEntity(this CreateIntegrationUserRequest request)
        {
            return new IntegrationUserEF
            { 
                Name = request.Name
            };
        }
    }
}
