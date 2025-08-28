namespace ASW.RemoteViewing.Shared.Constants;

public static class NotificationHubEvents
{
    public static string PostUsersChange { get; private set; } = "PostUsersChange";
    public static string PostChange { get; private set; } = "PostChange";
    public static string UsersChange { get; private set; } = "UsersChange";
    public static string CamerasChange { get; private set; } = "CamerasChange";
    public static string WeighingChange { get; private set; } = "WeighingChange";
    public static string EmptyWeighingChange { get; private set; } = "EmptyWeighingChange";
    public static string RFIDScanned { get; private set; } = "RFIDScanned";
    public static string NumberRecognized { get; private set; } = "NumberRecognized";
    public static string NumbersRecognized { get; private set; } = "NumbersRecognized";
}
