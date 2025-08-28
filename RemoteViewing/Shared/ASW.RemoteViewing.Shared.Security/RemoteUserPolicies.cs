namespace ASW.RemoteViewing.Shared.Security;

public static partial class Policies
{
    public static class RemoteUser
    {
        public const string CanView = "CanViewRemoteUser";
        public const string CanEdit = "CanEditRemoteUser";
        public const string CanAdd = "CanAddRemoteUser";
        public const string CanDelete = "CanDeleteRemoteUser";
    }
}
