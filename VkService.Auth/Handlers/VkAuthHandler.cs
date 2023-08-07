using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace VkService.Auth.Handlers;

public sealed class VkAuthHandler : OAuthHandler<VkAuthOptions>
{
    public VkAuthHandler(IOptionsMonitor<VkAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    public VkAuthHandler(IOptionsMonitor<VkAuthOptions> options, ILoggerFactory logger, UrlEncoder encoder) : base(options, logger, encoder)
    {
    }

    protected override async Task<AuthenticationTicket> CreateTicketAsync(ClaimsIdentity identity, AuthenticationProperties properties, OAuthTokenResponse tokens)
    {
        var url = QueryHelpers.AddQueryString(Options.UserInformationEndpoint,
            new Dictionary<string, string?>
            {
                ["v"] = Options.Version,
                ["access_token"] = tokens.AccessToken
            });
        var request = new HttpRequestMessage(HttpMethod.Get, url);

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await Backchannel.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
        response.EnsureSuccessStatusCode();

        var user = JsonNode.Parse(await response.Content.ReadAsStreamAsync());
        var context = new OAuthCreatingTicketContext(new ClaimsPrincipal(identity), properties, Context, Scheme,
            Options, Backchannel, tokens, user["response"][0].Deserialize<JsonElement>());

        context.RunClaimActions();
        context.Identity.AddClaim(new Claim(ClaimTypes.SerialNumber, context.AccessToken));

        await Options.Events.CreatingTicket(context);

        return new AuthenticationTicket(context.Principal, context.Properties, Scheme.Name);
    }
}