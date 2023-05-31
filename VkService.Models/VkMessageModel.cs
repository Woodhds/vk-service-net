namespace VkService.Models;

public record VkMessageModel
{
    public int Id { get; init; }
    public int OwnerId { get; init; }
    public DateTimeOffset Date { get; init; }
    public IEnumerable<string> Images { get; init; }
    public int LikesCount { get; init; }
    public string Owner { get; init; }
    public int RepostsCount { get; init; }
    public string Text { get; init; }
    public bool UserReposted { get; init; }
}