using System.Text.Json;

namespace VkService.Client.Abstractions;

public interface IJsonSerializerOptionsProvider
{
    JsonSerializerOptions Apply(JsonSerializerOptions? options);
}
