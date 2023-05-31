namespace VkService.Models;

public sealed record VkUserModel
{
    public int Id { get; init; }
    public string? Name { get; init; }
    public string? Avatar { get; init; }
}