namespace ASW.RemoteViewing.Features.Authorization.UserContext;

public interface IUserContext
{
    Guid? UserId { get; }
    string? UserName { get; }
}
