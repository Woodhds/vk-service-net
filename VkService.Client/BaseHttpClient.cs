using System.Collections.Specialized;
using System.Net.Mime;
using System.Runtime.ExceptionServices;
using System.Text;
using Microsoft.Extensions.Logging;
using VkService.Client.Abstractions;
using VkService.Infrastructure.Extensions;

namespace VkService.Client;

public class BaseHttpClient<TClient>
{
    private readonly IVkClient _httpClient;
    protected readonly ILogger<TClient> Logger;
    private readonly IJsonSerializer _jsonSerializer;

    public BaseHttpClient(
        IVkClient client,
        IJsonSerializer jsonSerializer,
        ILogger<TClient> logger
    )
    {
        _jsonSerializer = jsonSerializer;
        Logger = logger;
        _httpClient = client;
    }

    protected virtual async Task<T?> PostAsync<T>(string url, object? content, NameValueCollection? @params = null)
    {
        try
        {
            var uri = @params.BuildUrl(url);
            var stringContent =
                new StringContent(
                    _jsonSerializer.Serialize(content),
                    Encoding.UTF8,
                    MediaTypeNames.Application.Json
                );
            var response = await _httpClient.PostAsync(uri, stringContent).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            return _jsonSerializer.Deserialize<T>(await response.Content.ReadAsStringAsync().ConfigureAwait(false));
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            ExceptionDispatchInfo.Capture(e).Throw();
            return default;
        }
    }

    protected virtual async Task<T> GetAsync<T>(string url,
        NameValueCollection? @params = null)
    {
        try
        {
            var uri = @params.BuildUrl(url);
            return _jsonSerializer.Deserialize<T>(await _httpClient.GetStringAsync(uri).ConfigureAwait(false));
        }
        catch (Exception e)
        {
            Logger.LogError(e, e.Message);
            ExceptionDispatchInfo.Capture(e).Throw();
            return default;
        }
    }
}
