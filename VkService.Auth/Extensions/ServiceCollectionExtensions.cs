using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using VkService.Auth.Handlers;
using VkService.Auth.Services;

namespace VkService.Auth.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddVkAuth(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<VkAuthHandler>();
        services.Configure<VkAuthOptions>(configuration.GetSection("VkAuthOptions"));
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        services.AddHttpContextAccessor();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddCookie(z =>
            {
                z.LoginPath = "/auth/login";
            })
            .AddJwtBearer(z =>
            {
                z.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                };
            })
            .AddOAuth<VkAuthOptions, VkAuthHandler>("vkontakte", d =>
            {
                configuration.GetSection("VkAuthOptions").Bind(d);
                d.CallbackPath = "/sign-in-vkontakte";
                d.SaveTokens = true;
                d.ClaimActions.MapJsonKey(ClaimTypes.NameIdentifier, "id");
                d.ClaimActions.MapJsonKey(ClaimTypes.Name, "first_name");
                d.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "last_name");
            });

        services.AddAuthorization(ops =>
        {
            var defaultAuthorizationPolicyBuilder = new AuthorizationPolicyBuilder(
                JwtBearerDefaults.AuthenticationScheme,
                "vkontakte");
            defaultAuthorizationPolicyBuilder =
                defaultAuthorizationPolicyBuilder.RequireAuthenticatedUser();
            ops.DefaultPolicy = defaultAuthorizationPolicyBuilder.Build();
        });
    }
}