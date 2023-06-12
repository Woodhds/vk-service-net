using VkService.Client.Abstractions;

namespace VkService.Client.Services;

public class VkClient : IVkClient
{
    private readonly HttpClient _httpClient;
        
    public VkClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<string> GetStringAsync(string requestUri)
    {
        return _httpClient.GetStringAsync(requestUri);
    }

    public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content)
    {
        return _httpClient.PostAsync(requestUri, content);
    }
}
