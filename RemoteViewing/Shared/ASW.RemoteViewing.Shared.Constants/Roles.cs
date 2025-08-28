namespace ASW.RemoteViewing.Shared.Constants;

public static class Roles
{
    public const string Admin = "Admin"; 
    public const string Viewing = "Viewing"; 
    public const string Integration = "Integration"; 
    public const string PlaceClient = "PlaceClient"; 

    private static readonly Dictionary<string, string> rolesRu = new()
{
    { Admin, "Администратор" }, 
    { Viewing, "Просмотр" }, 
};
    public static string? GetRuRole(string role)
    {
        return rolesRu.TryGetValue(role, out var value) ? value : null;
    }
    public static string? GetRole(string ruRole)
    {
        return rolesRu.FirstOrDefault(x => x.Value == ruRole).Key;
    }
    public static IReadOnlyList<string> GetAllRoles()
    {
        return [.. rolesRu.OrderBy(x => x.Value).Select(x => x.Value)];
    }
}
