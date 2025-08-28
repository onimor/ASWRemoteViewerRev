namespace ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;

public interface IEndpointDefinition
{
    void DefineServices(IServiceCollection services);
    void DefineEndpoints(WebApplication app);
}
