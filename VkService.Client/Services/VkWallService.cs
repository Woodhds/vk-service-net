using System.Collections.Specialized;
using Microsoft.Extensions.Logging;
using VkService.Client.Abstractions;
using VkService.Models;

namespace VkService.Client.Services;

public class VkWallService : BaseHttpClient<VkWallService>, IVkWallService
{
        
    public VkWallService(IVkClient client, IJsonSerializer jsonSerializer, ILogger<VkWallService> logger) : base(client, jsonSerializer, logger)
    {
    }
        
    public Task<VkResponse<IReadOnlyCollection<VkMessage>>> WallSearch(string id, int skip, int take,
        string? search = null)
    {
        var @params = new NameValueCollection
        {
            {"count", take.ToString()},
            {"offset", ((skip - 1) * take).ToString()},
            {"filter", "owner"},
            {"owner_id", id}
        };
        var method = VkApiUrls.Wall;
        if (!string.IsNullOrEmpty(search))
        {
            method = VkApiUrls.WallSearch;
            @params.Add("query", search);
        }

        return base.GetAsync<VkResponse<IReadOnlyCollection<VkMessage>>>(method, @params);
    }

    public Task<VkResponse<IReadOnlyCollection<VkMessage>>> GetById(IEnumerable<RepostMessage>? vkRepostViewModels)
    {
        if (vkRepostViewModels == null)
        {
            throw new ArgumentNullException(nameof(vkRepostViewModels));
        }

        var @params = new NameValueCollection
        {
            {"posts", string.Join(",", vkRepostViewModels.Select(c => $"{c.OwnerId}_{c.Id}"))},
            {"extended", 1.ToString()},
            {"fields", "is_member"}
        };
        return base.GetAsync<VkResponse<IReadOnlyCollection<VkMessage>>>(VkApiUrls.WallGetById, @params);
    }

    public Task<SimpleVkResponse<VkRepostMessage>> Repost(RepostMessage model)
    {
        var @params = new NameValueCollection
        {
            {"object", $"wall{model.OwnerId}_{model.Id}"}
        };

        return base.PostAsync<SimpleVkResponse<VkRepostMessage>>(VkApiUrls.Repost, null, @params);
    }
}
