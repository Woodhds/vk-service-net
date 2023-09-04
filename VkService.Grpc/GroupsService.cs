using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using VkService.Client.Abstractions;
using VkService.Data;

namespace VkService.Grpc;

public class GroupsService : global::GroupsService.GroupsServiceBase
{
    private readonly IDbConnectionFactory _factory;
    private readonly IVkGroupService _groupService;

    public GroupsService(IDbConnectionFactory factory, IVkGroupService groupService)
    {
        _factory = factory;
        _groupService = groupService;
    }

    public override async Task<Empty> AddBanned(AddBannedRequest request, ServerCallContext context)
    {
        await using var connection = _factory.GetConnection();

        if (request.Ids != null)
        {
            foreach (var id in request.Ids)
            {
                await connection.ExecuteAsync(
                    new CommandDefinition("INSERT OR IGNORE INTO BannedGroups (Id) VALUES (@id)",
                        new { id },
                        cancellationToken: context.CancellationToken));
            }
        }

        return new Empty();
    }

    public override async Task<GetBannedResponse> GetBanned(GetBannedRequest request, ServerCallContext context)
    {
        await using var connection = _factory.GetConnection();

        var groups = (await connection.QueryAsync<int>(
                new("select Id from BannedGroups",
                    cancellationToken: context.CancellationToken)))
            .ToArray();

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
