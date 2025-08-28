using AspNetCore.Authentication.ApiKey;
using ASW.RemoteViewing.Features.Authorization;
using ASW.RemoteViewing.Features.Authorization.Claims;
using ASW.RemoteViewing.Features.Authorization.Policy;
using ASW.RemoteViewing.Features.IntegrationUser.Services;
using ASW.RemoteViewing.Infrastructure.Security;
using ASW.RemoteViewing.Infrastructure.Security.Key;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace ASW.RemoteViewing.Infrastructure.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddCustomAuthentication(this IServiceCollection services, KeyService keyService)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = AuthSchemes.UserJwt;
        })
        .AddJwtBearer(AuthSchemes.UserJwt, options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateActor = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "ASW",
                ValidAudience = "UserAccess",
                IssuerSigningKey = new RsaSecurityKey(keyService.RsaKey1)
            };
        })
        .AddJwtBearer(AuthSchemes.PlaceUserJwt, options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateActor = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                ValidateLifetime = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = "ASW",
                ValidAudience = "PlaceClientAccess",
                IssuerSigningKey = new RsaSecurityKey(keyService.RsaKey3)
            };

            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    if (!context.HttpContext.Request.Path.StartsWithSegments("/api"))
                    {
                        context.NoResult();
                    }
                    return Task.CompletedTask;
                }
            };
        })
        .AddApiKeyInHeader(AuthSchemes.IntegrationUser, options =>
        {
            options.Realm = "ASW";
            options.KeyName = "X-Api-Key";
            options.Events = new ApiKeyEvents
            {
                OnValidateKey = async context =>
                {
                    if (!context.HttpContext.Request.Path.StartsWithSegments("/api"))
                    {
                        context.NoResult(); 
                    }

                    var integrationUserService = context.HttpContext.RequestServices.GetRequiredService<IIntegrationUserService>();
                    var timeProvider = context.HttpContext.RequestServices.GetRequiredService<TimeProvider>();
                    var integrationUser = await integrationUserService.GetByKey(context.ApiKey);

                    if (integrationUser is null)
                    {
                        context.NoResult(); 
                    }
                     
                    var claims = ClaimsPrincipalFactory.CreateClaims(new AuthClaimsInfo
                    {
                        Id = integrationUser!.Id,
                        Name = integrationUser.Name,
                        Role = integrationUser.Role,
                        ModifiedId = Guid.Empty,
                        AuthScheme = AuthSchemes.IntegrationUser
                    }); 
                    context.Principal = new ClaimsPrincipal(new ClaimsIdentity(claims, context.Scheme.Name)); 
                }
            };
        });

        return services;
    }

    public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
    {
        var builder = services.AddAuthorizationBuilder()
            .SetDefaultPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddRequirements(new ModifiedIdRequirement())
                .Build());
        
        foreach (var policyConfig in PolicyRegistry.GetAll())
        {
            builder.AddPolicy(policyConfig.Name, policy =>
            {
                policy.AddAuthenticationSchemes(policyConfig.Schemes);
                policy.RequireAuthenticatedUser();

                if (policyConfig.RequiresModifiedId)
                    policy.AddRequirements(new ModifiedIdRequirement());

                if (policyConfig.Roles?.Length > 0)
                    policy.RequireRole(policyConfig.Roles);
            });
        }
        return services;
    }
}
