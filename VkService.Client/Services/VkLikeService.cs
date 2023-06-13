using System.Collections.Specialized;
using Microsoft.Extensions.Logging;
using VkService.Client.Abstractions;
using VkService.Models;

namespace VkService.Client.Services;

public sealed class VkLikeService : BaseHttpClient<VkLikeService>, IVkLikeService
{
    public VkLikeService(
        IVkClient client,
        IJsonSerializer jsonSerializer,
        ILogger<VkLikeService> logger
    ) : base(client, jsonSerializer, logger)
    {
    }

    public Task<SimpleVkResponse<VkResponseLikeModel>> Like(RepostMessage model)
    {
        var @params = new NameValueCollection
        {
            {"owner_id", $"{model.OwnerId}"},
            {"item_id", $"{model.Id}"},
            {"type", "post"}
        };
        
        return GetAsync<SimpleVkResponse<VkResponseLikeModel>>(VkApiUrls.Like, @params);
    }
}
