namespace ASW.RemoteViewing.Shared.Security;

public record PolicyConfig(
 string Name,
 string[] Schemes,
 string[] Roles,
 bool RequiresModifiedId
);
