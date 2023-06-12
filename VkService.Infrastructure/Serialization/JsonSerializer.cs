using System.Text.Json;
using VkService.Client.Abstractions;

namespace VkService.Infrastructure.Serialization;

public class JsonSerializer : IJsonSerializer
{
    private readonly JsonSerializerOptions _jsonSerializerOptions;

    public JsonSerializer(IJsonSerializerOptionsProvider jsonSerializerOptionsProvider)
    {
        _jsonSerializerOptions = jsonSerializerOptionsProvider.Apply(new JsonSerializerOptions());
    }

    public T? Deserialize<T>(string input)
    {
        return string.IsNullOrEmpty(input)
            ? default
            : System.Text.Json.JsonSerializer.Deserialize<T>(input,
                _jsonSerializerOptions
            );
    }

    public string Serialize<T>(T? value)
    {
        return System.Text.Json.JsonSerializer.Serialize(value, _jsonSerializerOptions);
    }
}
