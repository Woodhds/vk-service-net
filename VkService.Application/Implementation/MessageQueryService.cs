using Dapper;
using VkService.Application.Abstractions;
using VkService.Data;
using VkService.Models;

namespace VkService.Application.Implementation;

public sealed class MessageQueryService : IMessagesQueryService
{
    private readonly IDbConnectionFactory _factory;

    public MessageQueryService(IDbConnectionFactory factory)
    {
        _factory = factory;
    }

    public async Task<IReadOnlyCollection<VkMessageModel>> GetMessages(string search)
    {
        await using var connection = _factory.GetConnection();

        var searchText = $"\"{search}\"";

        var messages = await connection.QueryAsync<VkMessageModel>(new(
            """
            SELECT
                messages.Id,
            	messages.OwnerId
            FROM messages 
            INNER JOIN messages_search as search  on messages.RowId = search.RowId
            LEFT JOIN BannedGroups on messages.OwnerId = BannedGroups.Id
            	where search.Text MATCH @searchText AND BannedGroups.Id IS NULL
            	order by rank
            """, new { searchText }));

        return messages.ToArray();
    }
}
