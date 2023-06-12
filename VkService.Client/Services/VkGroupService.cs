using System.Collections.Specialized;
using Microsoft.Extensions.Logging;
using VkService.Client.Abstractions;
using VkService.Models;

namespace VkService.Client.Services;

public class VkGroupService : BaseHttpClient<VkGroupService>, IVkGroupService
{
    public VkGroupService(
        IVkClient client,
        IJsonSerializer jsonSerializer,
        ILogger<VkGroupService> logger
    ) : base(client, jsonSerializer, logger)
    {
    }
        
    public async Task JoinGroup(int groupId)
    {
        var @params = new NameValueCollection
        {
            {"group_id", groupId.ToString()}
        };

        await base.GetAsync<SimpleVkResponse<int>>(VkApiUrls.GroupJoin, @params);
    }

    public Task<VkResponse<IReadOnlyCollection<VkGroup>>> GetGroups(int count, int offset)
    {
        var @params = new NameValueCollection
        {
            {"count", $"{count}"},
            {"fields", "name, description"},
            {"extended", "1"},
            {"offset", $"{offset}"}
        };
        return base.GetAsync<VkResponse<IReadOnlyCollection<VkGroup>>>(VkApiUrls.Groups, @params);
    }

    public Task LeaveGroup(int groupId)
    {
        var @params = new NameValueCollection
        {
            {"group_id", $"{groupId}"}
        };
        return base.GetAsync<SimpleVkResponse<string>>(VkApiUrls.LeaveGroup, @params);
    }
}
