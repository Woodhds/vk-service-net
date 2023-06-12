using System.Text.Json;
using System.Text.Json.Serialization;
using VkService.Client.Abstractions;

namespace VkService.Client.Infrastructure.Serialization;

public class JsonSerializerOptionsProvider : IJsonSerializerOptionsProvider
{
    public JsonSerializerOptions Apply(JsonSerializerOptions? options)
    {
        options ??= new JsonSerializerOptions();

        options.Converters.Add(new JsonStringEnumConverter());
        options.PropertyNameCaseInsensitive = true;
        options.PropertyNamingPolicy = null;

        return options;
    }
}
