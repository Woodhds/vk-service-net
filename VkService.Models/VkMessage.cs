using System.Text.Json.Serialization;
using VkService.Client.Abstractions.Infrastructure.Converters;

namespace VkService.Models;

public record VkMessage
{
    public int Id { get; set; }
    [JsonPropertyName("owner_id")] public int OwnerId { get; set; }
    [JsonPropertyName("from_id")] public int FromId { get; set; }

    [JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime Date { get; set; }

    public string? Text { get; set; }
    [JsonPropertyName("copy_history")] public List<VkMessage>? CopyHistory { get; set; } = new();
    public List<MessageAttachment> Attachments { get; set; } = new();
    public MessageReposts? Reposts { get; set; }
    public VkLike? Likes { get; set; }
}

public enum PostType
{
    Post = 0,
    Video = 1
}

public enum MessageAttachmentType
{
    photo = 0,
    audio = 1,
    video = 2,
    link = 3,
    poll = 4,
    page = 5,
    album = 6,
    doc = 7,
    posted_photo = 8,
    graffiti = 9,
    note = 10,
    app = 11,
    photos_list = 12,
    market = 13,
    market_album = 14,
    sticker = 15,
    pretty_cards = 16,
    Event = 17,
    narrative = 18
}

public class MessageAttachment
{
    public MessageAttachmentType Type { get; set; }
    public AttachmentPhoto? Photo { get; set; }
}

public class AttachmentPhoto
{
    public uint id { get; set; }
    public List<PhotoSize> Sizes { get; set; } = new List<PhotoSize>();
}

public class PhotoSize
{
    public uint Width { get; set; }
    public uint Height { get; set; }
    public PhotoSizeType Type { get; set; }
    public string Url { get; set; }
}

public enum PhotoSizeType
{
    m = 0,
    o = 1,
    p = 2,
    q = 3,
    r = 4,
    s = 5,
    x = 6,
    y = 7,
    z = 8,
    w = 9
}

public record Owner
{
    public int Id { get; set; }
    [JsonPropertyName("screen_name")] public string ScreenName { get; set; }
}

public record Profile : Owner
{
    [JsonPropertyName("first_name")] public string FirstName { get; set; }
    [JsonPropertyName("last_name")] public string LastName { get; set; }
}
