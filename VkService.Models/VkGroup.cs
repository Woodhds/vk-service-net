using System.Text.Json.Serialization;
using VkService.Client.Abstractions.Infrastructure.Converters;

namespace VkService.Models;

public record VkGroup : Owner
{
    public string Name { get; set; }
    public string Description { get; set; }
    [JsonConverter(typeof(JsonBooleanConverter))]
    [JsonPropertyName("is_member")]
    public bool IsMember { get; set; }
}
