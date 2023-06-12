using System.Text.Json.Serialization;
using VkService.Client.Abstractions.Infrastructure.Converters;

namespace VkService.Models;

public record VkLike
{
    public int Count { get; set; }
    [JsonConverter(typeof(JsonBooleanConverter))]
    [JsonPropertyName("user_likes")]
    public bool UserLikes { get; set; }
}
