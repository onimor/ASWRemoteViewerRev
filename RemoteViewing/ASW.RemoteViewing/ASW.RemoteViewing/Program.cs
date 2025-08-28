using ASW.RemoteViewing.Api.Hub;
using ASW.RemoteViewing.Client.Shared;
using ASW.RemoteViewing.Components;
using ASW.RemoteViewing.Features.Authorization;
using ASW.RemoteViewing.Features.Authorization.CurrentUser.Default;
using ASW.RemoteViewing.Features.Authorization.CurrentUser.IntegrationUser;
using ASW.RemoteViewing.Features.Authorization.CurrentUser.PlaceUser;
using ASW.RemoteViewing.Features.Authorization.Policy;
using ASW.RemoteViewing.Features.Authorization.UserContext;
using ASW.RemoteViewing.Features.CurrentUser.Default;
using ASW.RemoteViewing.Features.RemoteCamera.Mapping;
using ASW.RemoteViewing.Infrastructure.Data;
using ASW.RemoteViewing.Infrastructure.Extensions;
using ASW.RemoteViewing.Infrastructure.Extensions.EndpointExtensions;
using ASW.RemoteViewing.Infrastructure.Security.Key;
using ASW.RemoteViewing.Shared.Middleware;
using ASW.RemoteViewing.Shared.Utilities.JsonConverters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args); 
builder.Services.AddDbContext<PgDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddSignalR();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<ICurrentUserProvider, CurrentUserProvider>();
builder.Services.AddScoped<ICurrentPlaceUserProvider, CurrentPlaceUserProvider>();
builder.Services.AddScoped<ICurrentIntegrationUserProvider, CurrentIntegrationUserProvider>();
builder.Services.AddScoped<IAuthorizationHandler, ModifiedIdHandler>(); 

builder.Services.AddSingleton<MudThemeHolder>();
builder.Services.AddRazorComponents()
    .AddInteractiveWebAssemblyComponents();
builder.Services.AddMudServices();

builder.Services.AddEndpointDefinitions(typeof(IEndpointDefinition));

var keyService_ = new KeyService();
builder.Services.AddSingleton(keyService_);
builder.Services
    .AddCustomAuthentication(keyService_)
    .AddCustomAuthorization();

builder.Services.AddAutoMapper(typeof(RemoteCameraMappingProfile).Assembly);
builder.Services.Configure<JsonOptions>(options =>
{ 
    options.SerializerOptions.Converters.Add(new StrictIso8601DateTimeConverter());
});
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "ASW Remote API",
        Version = "v1",
        Description = "Документация API с использованием токена X-Api-Key"
    });

    // Добавляем поддержку X-Api-Key
    c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
    {
        Description = "Введите токен в заголовке X-Api-Key",
        Type = SecuritySchemeType.ApiKey,
        Name = "X-Api-Key",
        In = ParameterLocation.Header,
        Scheme = AuthSchemes.IntegrationUser
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "ApiKey" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "ASW Remote API v1");
    c.RoutePrefix = "swagger";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseExceptionHandler();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
   
}

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication();
app.UseAuthorization();

//Ловим все не существующие пути к API, остальные не существующие пропускаем на фронтенд
var api = app.MapGroup("/api");
api.Map("{**path}", (HttpContext ctx, string path) =>
    Results.Problem(
        title: "Маршрут не найден",
        detail: $"Путь '{path}' не соответствует ни одному API endpoint.",
        statusCode: StatusCodes.Status404NotFound,
        instance: ctx.Request.Path
    )
).ExcludeFromDescription(); 

app.MapRazorComponents<App>()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(ASW.RemoteViewing.Client._Imports).Assembly); 
app.MapHub<NotificationHub>("/api/v1/NotificationHub").AllowAnonymous();
app.UseEndpointDefinitions(); 

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<PgDbContext>();
    await db.Database.EnsureCreatedAsync();
}

app.Run();


