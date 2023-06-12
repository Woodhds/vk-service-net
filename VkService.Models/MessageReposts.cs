using System.Text.Json.Serialization;
using VkService.Client.Abstractions.Infrastructure.Converters;

namespace VkService.Models;

public record MessageReposts
{
    [JsonConverter(typeof(JsonBooleanConverter))]
    [JsonPropertyName("user_reposted")]
    public bool UserReposted { get; set; }
    public int Count { get; set; }
}
