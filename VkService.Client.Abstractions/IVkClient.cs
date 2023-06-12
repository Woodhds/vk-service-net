namespace VkService.Client.Abstractions;

public interface IVkClient
{
    Task<string> GetStringAsync(string requestUri);
    Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
}
