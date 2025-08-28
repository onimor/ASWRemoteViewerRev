using ASW.RemoteViewing.Client.Services.Token;

namespace ASW.RemoteViewing.Client.Infrastructure.Handler;

public class AuthorizationHandler : DelegatingHandler
{
    private readonly ITokenProvider _tokenProvider;

    public AuthorizationHandler(ITokenProvider tokenProvider)
    {
        _tokenProvider = tokenProvider;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _tokenProvider.GetTokenAsync();

        if (!string.IsNullOrWhiteSpace(token))
        {
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
