using ASW.RemoteViewing.Client.Clients;
using ASW.RemoteViewing.Client.Infrastructure.Authorization;
using ASW.RemoteViewing.Client.Infrastructure.Handler;
using ASW.RemoteViewing.Client.Infrastructure.Http;
using ASW.RemoteViewing.Client.Infrastructure.Providers;
using ASW.RemoteViewing.Client.Infrastructure.Registrations;
using ASW.RemoteViewing.Client.Services.Auth;
using ASW.RemoteViewing.Client.Services.Token;
using ASW.RemoteViewing.Client.Services.UserContext;
using ASW.RemoteViewing.Client.Shared;
using BlazorAnimate;
using Blazored.LocalStorage;
using Blazored.SessionStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.Configure<AnimateOptions>(options =>
{
    options.Animation = Animations.FadeRight;
    options.Duration = TimeSpan.FromMilliseconds(250);
});
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddTransient<AuthorizationHandler>();
builder.Services.AddScoped(sp =>
{
    var nav = sp.GetRequiredService<NavigationManager>();

    // Без AuthorizationHandler!
    return new TokenValidationHttpClient(new HttpClient
    {
        BaseAddress = new Uri(nav.BaseUri)
    });
});
builder.Services.AddScoped(sp =>
{
    var nav = sp.GetRequiredService<NavigationManager>();
    var tokenProvider = sp.GetRequiredService<ITokenProvider>();

    var handler = new AuthorizationHandler(tokenProvider)
    {
        InnerHandler = new HttpClientHandler() 
    };

    var httpClient = new HttpClient(handler)
    {
        BaseAddress = new Uri(nav.BaseUri)
    };

    return httpClient;
});

builder.Services.AddScoped<SafeHttpClient>(); 
builder.Services.AddRemoteClients();
builder.Services.AddMudServices();
builder.Services.AddScoped<AuthClient>(); 
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>(); // отдельно, чтобы не кастить
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddClientAuthorizationPolicies();
builder.Services.AddSingleton<MudThemeHolder>();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();
await builder.Build().RunAsync();
