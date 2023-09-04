namespace VkService.Data.Entities;

public class VkMessage
{
    public int MessageId { get; set; }
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public int RepostedFrom { get; set; }
    public DateTimeOffset Date { get; set; }
}
