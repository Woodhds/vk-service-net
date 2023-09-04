using Dapper;
using VkService.Application.Abstractions;
using VkService.Data;
using VkEntity = VkService.Data.Entities.VkMessage;

namespace VkService.Application.Implementation;

public sealed class MessagesSaveService : IMessagesSaveService
{
    private readonly IDbConnectionFactory _factory;

    public MessagesSaveService(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task Save(IEnumerable<Models.VkMessage> message, CancellationToken cancellationToken)
    {
        await using var connection = _factory.GetConnection();

        await connection.OpenAsync(cancellationToken);

        await using var ts = await connection.BeginTransactionAsync(cancellationToken);

        var bannedGroups = (await connection.QueryAsync<int>("select Id from BannedGroups")).ToArray();

        var toAdd = message.ExceptBy(bannedGroups, f => f.OwnerId).ToArray();

        foreach (var add in toAdd)
        {
            var rowId = await connection.ExecuteScalarAsync<int>(new(
                """
                    INSERT OR IGNORE INTO Messages (Id, OwnerId, Date, RepostedFrom) VALUES (@Id, @OwnerId, @Date, @RepostedFrom)
                    RETURNING RowId
                """, new
                {
                    add.Id,
                    add.OwnerId,
                    add.Date,
                    RepostedFrom = add.FromId
                },
                cancellationToken: cancellationToken));

            if (rowId > 0)
            {
                await connection.ExecuteAsync(new(
                    "INSERT OR IGNORE INTO messages_search (RowId, Text) VALUES (@RowId, @Text)", new
                    {
                        RowId = rowId,
                        add.Text
                    }, cancellationToken: cancellationToken));
            }
        }

        await ts.CommitAsync(cancellationToken);
    }
}
