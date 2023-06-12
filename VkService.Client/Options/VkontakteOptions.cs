namespace VkService.Client.Options;

public record VkontakteOptions
{
    public string ApiVersion { get; init; } = "";

    public string AppId { get; init; } = "";
    public string AppSecret { get; init; } = "";
    public string AppScope { get; init; } = "";
    public string Token { get; init; } = "";

    public HashSet<string> Fields { get; init; } = new();
}
