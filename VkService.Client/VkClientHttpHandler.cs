using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using VkService.Client.Abstractions;
using VkService.Client.Options;

namespace VkService.Client;

public class VkClientHttpHandler : DelegatingHandler
{
    private readonly IOptions<VkontakteOptions> _vkontakteOptions;
    private readonly IUserTokenAccessor _vkTokenAccessor;
    private static readonly SemaphoreSlim _sync = new (1, 1);

    public VkClientHttpHandler(
        IOptions<VkontakteOptions> vkontakteOptions,
        IUserTokenAccessor vkTokenAccessor
    )
    {
        _vkontakteOptions = vkontakteOptions;
        _vkTokenAccessor = vkTokenAccessor;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken
    )
    {
        var url = QueryHelpers.AddQueryString(request.RequestUri.ToString(),
            new Dictionary<string, string>
            {
                ["v"] = _vkontakteOptions.Value.ApiVersion,
                ["access_token"] = await _vkTokenAccessor.GetTokenAsync()
            });
        request.RequestUri = new Uri(url);

        try
        {
            await _sync.WaitAsync(cancellationToken);

            var response = await base.SendAsync(request, cancellationToken);
            await Task.Delay(500, cancellationToken);

            return response;
        }
        finally
        {
            _sync.Release();
        }
    }
}
