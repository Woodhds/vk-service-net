using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using VkService.Application.Abstractions;
using VkService.Data;
using VkService.Models;
using DbFunctionsExtensions = VkService.Data.Extensions.DbFunctionsExtensions;

namespace VkService.Application.Implementation;

public sealed class MessageQueryService : IMessagesQueryService
{
    private readonly IDbContextFactory<DataContext> _factory;

    public MessageQueryService(IDbContextFactory<DataContext> factory)
    {
        _factory = factory;
    }

    public async Task<IReadOnlyCollection<VkMessageModel>> GetMessages(string search)
    {
        await using var context = await _factory.CreateDbContextAsync();

        var searchText = $"\"{search}\"";

        var messages = context.MessageSearch
            .Include(f => f.Message)
            .Where(f => DbFunctionsExtensions.Match(searchText, f.Text))
            .OrderBy(f => f.Rank)
            .Select(f => new VkMessageModel
            {
                OwnerId = f.Message.OwnerId,
                Id = f.Message.Id
            })
            .ToArray();

        return messages;
    }
}
