using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using VkService.Auth.Services;

namespace VkService.Auth.Apis;

public static class AuthEndpoints
{
    public static void AddAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        endpoints.MapGet("api/callback", ([FromServices]IJwtGenerator generator) =>
        {
            Results.Ok(generator.Generate());
        })
        .RequireAuthorization();

        endpoints.MapGet("auth/login", () =>
        {
            var authProperties = new AuthenticationProperties()
            {
                RedirectUri = "api/callback"
            };

            return Results.Challenge(authProperties, new[] { "vkontakte" });
        });
    }
}