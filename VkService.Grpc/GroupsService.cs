using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using VkService.Client.Abstractions;
using VkService.Data;
using VkService.Data.Entities;

namespace VkService.Grpc;

public class GroupsService : global::GroupsService.GroupsServiceBase
{
    private readonly IDbContextFactory<DataContext> _factory;
    private readonly IVkGroupService _groupService;

    public GroupsService(IDbContextFactory<DataContext> factory, IVkGroupService groupService)
    {
        _factory = factory;
        _groupService = groupService;
    }

    public override async Task<Empty> AddBanned(AddBannedRequest request, ServerCallContext context)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        var existed = await ctx.BannedGroups
            .Where(f => request.Ids.Contains(f.Id))
            .Select(f => f.Id)
            .ToArrayAsync(context.CancellationToken);

        var toAdd = request.Ids.Except(existed).ToArray();
        if (toAdd.Length == 0)
            return new Empty();

        ctx.BannedGroups.AddRange(toAdd.Select(t => new BannedGroup { Id = t }));
        await ctx.SaveChangesAsync(context.CancellationToken);

        return new Empty();
    }

    public override async Task<GetBannedResponse> GetBanned(GetBannedRequest request, ServerCallContext context)
    {
        await using var ctx = await _factory.CreateDbContextAsync();
        var groups = await ctx.BannedGroups
            .Select(a => a.Id)
            .ToArrayAsync(context.CancellationToken);

        if (groups is { Length: 0 })
            return new GetBannedResponse();

        var vkGroups = await _groupService.GetById(groups, context.CancellationToken);

        return new GetBannedResponse
        {
            Messages =
            {
                vkGroups.Response?.Items.IntersectBy(groups, f => f.Id)
                    .Select(e => new GetBannedResponse.Types.Banned
                    {
                        Id = e.Id,
                        Name = e.Name
                    })
            }
        };
    }
}