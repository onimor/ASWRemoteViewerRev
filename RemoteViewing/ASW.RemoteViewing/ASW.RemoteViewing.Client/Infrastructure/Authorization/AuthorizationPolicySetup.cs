using ASW.RemoteViewing.Shared.Constants;
using ASW.RemoteViewing.Shared.Security;

namespace ASW.RemoteViewing.Client.Infrastructure.Authorization;

public static class AuthorizationPolicySetup
{
    public static IServiceCollection AddClientAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorizationCore(options =>
        {
            options.AddPolicy(Policies.RemoteWeighing.CanView, policy =>
                policy.RequireRole(Roles.Admin, Roles.Viewing));

            options.AddPolicy(Policies.PlaceUser.CanView, policy =>
                policy.RequireRole(Roles.Admin)); 

            options.AddPolicy(Policies.IntegrationUser.CanView, policy =>
                policy.RequireRole(Roles.Admin)); 
        });

        return services;
    }
}
