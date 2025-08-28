using ASW.RemoteViewing.Client.Clients;

namespace ASW.RemoteViewing.Client.Infrastructure.Registrations;
public static class RemoteClientRegistration
{
    public static IServiceCollection AddRemoteClients(this IServiceCollection services)
    {
        services.AddScoped<RemoteAxesDistClient>();
        services.AddScoped<RemoteAxesVelClient>();
        services.AddScoped<RemoteAxesWeightClient>();
        services.AddScoped<RemoteCameraClient>();
        services.AddScoped<RemoteCarClient>();
        services.AddScoped<RemoteCounterpartyClient>();
        services.AddScoped<RemoteDriverClient>();
        services.AddScoped<RemoteEmptyWeighingClient>();
        services.AddScoped<RemoteGoodClient>();
        services.AddScoped<RemotePhotosClient>();
        services.AddScoped<RemotePostClient>();
        services.AddScoped<RemotePostUsersClient>();
        services.AddScoped<RemoteTrailerClient>();
        services.AddScoped<RemoteWeighingClient>(); 
        services.AddScoped<ExportClient>(); 
        services.AddScoped<PlaceUserClient>(); 
        services.AddScoped<IntegrationUserClient>(); 
        return services;
    }
}
