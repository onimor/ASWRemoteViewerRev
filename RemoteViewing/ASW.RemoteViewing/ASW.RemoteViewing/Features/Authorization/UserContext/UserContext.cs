using System.Security.Claims;

namespace ASW.RemoteViewing.Features.Authorization.UserContext;

public class UserContext: IUserContext
{
    private readonly IHttpContextAccessor _accessor;

    public UserContext(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public Guid? UserId
    {
        get
        {
            var idClaim = _accessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier);
            if (idClaim != null && Guid.TryParse(idClaim.Value, out var id))
                return id;
            return null;
        }
    }
    public string? UserName
    {
        get
        {
            var nameClaim = _accessor.HttpContext?.User?.FindFirst(ClaimTypes.Name);
            if (nameClaim != null)
                return nameClaim.Value;
            return null;
        }
    }
}
