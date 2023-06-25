using System.Diagnostics.CodeAnalysis;

namespace VkService.Models;

public class VkMessageModel : IEquatable<VkMessageModel>
{
    public int Id { get; set; }
    public int OwnerId { get; set; }
    public int FromId { get; private set; }
    public DateTime Date { get; private set; }
    public string? Text { get; private set; }
    public int LikesCount { get; private set; }
    public int RepostsCount { get; private set; }
    public string Owner { get; private set; }
    public string[] Images { get; private set; }
    public bool UserReposted { get; private set; }
    
    public bool UserLikes { get; private set; }

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
        UserLikes = message.Likes?.UserLikes ?? false;
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
