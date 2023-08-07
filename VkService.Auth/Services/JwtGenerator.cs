using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using VkService.Auth.Models;

namespace VkService.Auth.Services;

public sealed class JwtGenerator : IJwtGenerator
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public JwtGenerator(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IdentityUser? Generate()
    {
        if (_httpContextAccessor.HttpContext.User.Identity?.IsAuthenticated == true)
        {
            var signingKey = new byte[32];
            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(_httpContextAccessor.HttpContext.User.Claims),
                Expires = DateTimeOffset.UtcNow.Add(TimeSpan.FromHours(1)).DateTime,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(signingKey),
                    SecurityAlgorithms.HmacSha256),
            };

            JsonWebTokenHandler handler = new();
            var token = handler.CreateToken(tokenDescriptor);

            return new IdentityUser
            {
                FullName = _httpContextAccessor.HttpContext.User.Identity.Name,
                AccessToken = token
            };
        }

        return null;
    }
}