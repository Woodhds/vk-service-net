using System.Collections.Specialized;
using Microsoft.Extensions.Logging;
using VkService.Client.Abstractions;
using VkService.Models;

namespace VkService.Client.Services;

public sealed class VkUserService : BaseHttpClient<VkUserService>, IVkUserService
{
    public VkUserService(IVkClient client, IJsonSerializer jsonSerializer, ILogger<VkUserService> logger) : base(client, jsonSerializer, logger)
    {
    }

    public Task<VkResponse<IReadOnlyCollection<VkUserResponse>>> SearchUserAsync(string search)
    {
        var @params = new NameValueCollection
        {
            {"q", search},
            {"count", "100"},
            {"fields", "photo_50"}
        };

        return base.GetAsync<VkResponse<IReadOnlyCollection<VkUserResponse>>>(VkApiUrls.UserSearch, @params);
    }

    public Task<SimpleVkResponse<IReadOnlyCollection<VkUserResponse>>> GetUserInfo(string id)
    {
        var @params = new NameValueCollection
        {
            {"user_ids", id},
            {"fields", "first_name,last_name,photo_50"}
        };

        return base.GetAsync<SimpleVkResponse<IReadOnlyCollection<VkUserResponse>>>(VkApiUrls.UserInfo, @params);
    }
}
