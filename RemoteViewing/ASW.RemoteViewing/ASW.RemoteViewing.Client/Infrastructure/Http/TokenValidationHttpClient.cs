namespace ASW.RemoteViewing.Client.Infrastructure.Http;

public class TokenValidationHttpClient
{
    public HttpClient Client { get; }

    public TokenValidationHttpClient(HttpClient client)
    {
        Client = client;
    }
}
