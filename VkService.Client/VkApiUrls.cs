namespace VkService.Client;

public static class VkApiUrls
{
    public const string AuthorizeUrl = "https://oauth.vk.com/authorize";
    public const string Domain = "https://api.vk.com/method/";
    public const string Wall = Domain + "wall.get";
    public const string WallSearch = Domain + "wall.search";
    public const string Repost = Domain + "wall.repost";
    public const string WallGetById = Domain + "wall.getById";
    public const string GroupJoin = Domain + "groups.join";
    public const string UserInfo = Domain + "users.get";
    public const string Groups = Domain + "groups.get";
    public const string LeaveGroup = Domain + "groups.leave";
    public const string Like = Domain + "likes.add";
    public const string UserSearch = Domain + "users.search";
}
