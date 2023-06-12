namespace VkService.Models;

public class VkMessageModel : IEquatable<VkMessageModel>
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public int FromId { get; set; }
    public DateTime Date { get; set; }
    public string? Text { get; set; }
    public int LikesCount { get; set; }
    public int RepostsCount { get; set; }
    public string Owner { get; set; }
    public string[] Images { get; set; }

    public bool UserReposted { get; set; }

    public VkMessageModel()
    {
    }

    public VkMessageModel(VkMessage message, IEnumerable<VkGroup> groups)
    {
        Id = message.Id;
        OwnerId = message.OwnerId;
        Date = message.Date;
        Text = message.Text;
        FromId = message.FromId;
        Images = message.Attachments?
            .Where(f => f is { Type: MessageAttachmentType.photo, Photo.Sizes.Count: > 2 })
            .Select(f => f.Photo.Sizes[3].Url)
            .ToArray();
        LikesCount = message.Likes?.Count ?? 0;
        RepostsCount = message.Reposts?.Count ?? 0;
        UserReposted = message.Reposts?.UserReposted ?? false;
        Owner = groups.Where(a => a.Id == -message.OwnerId).Select(f => f.Name).FirstOrDefault() ?? "";
    }

    public bool Equals(VkMessageModel? other)
    {
        return OwnerId == other.OwnerId && Id == other.Id;
    }

    public override bool Equals(object? x)
    {
        if (!(x is VkMessageModel obj)) return false;

        return obj.OwnerId == OwnerId && Id == obj.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(OwnerId, Id);
    }
}
