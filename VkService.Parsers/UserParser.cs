﻿using System.Runtime.CompilerServices;
using Dapper;
using VkService.Client.Abstractions;
using VkService.Data;
using VkService.Models;
using VkService.Parsers.Abstractions;

namespace VkService.Parsers;

public sealed class UserParser : IMessageParser
{
    private readonly IDbConnectionFactory _factory;
    private readonly IVkWallService _wallService;

    public UserParser(IDbConnectionFactory factory, IVkWallService wallService)
    {
        _factory = factory;
        _wallService = wallService;
    }

    public async IAsyncEnumerable<IEnumerable<RepostMessage>> Parse([EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await using var connection = _factory.GetConnection();

        var ids = await connection.QueryAsync<int>("select Id from Users");
        foreach (var id in ids)
        {
            for (var i = 1; i <= 4; i++)
            {
                yield return await GetMessages(id.ToString(), i, 50);
            }
        }
    }

    private async Task<IEnumerable<RepostMessage>> GetMessages(string userId, int page, int count)
    {
        var data = await _wallService.WallSearch(userId, page, count);

        var reposts = data.Response.Items
            .OrderByDescending(c => c.Date)
            .Where(c => c.CopyHistory is { Count: > 0 })
            .Select(c => c.CopyHistory.First())
            .Distinct()
            .Select(f => new RepostMessage(f.OwnerId, f.Id));

        return reposts;
    }
}
