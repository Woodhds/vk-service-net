using Microsoft.EntityFrameworkCore;
using VkService.Application.Abstractions;
using VkService.Data;
using VkService.Data.Entities;
using VkEntity = VkService.Data.Entities.VkMessage;

namespace VkService.Application.Implementation;

public sealed class MessagesSaveService : IMessagesSaveService
{
    private readonly IDbContextFactory<DataContext> _factory;

    public MessagesSaveService(IDbContextFactory<DataContext> factory)
    {
        _factory = factory;
    }

    public async Task Save(IEnumerable<Models.VkMessage> message, CancellationToken cancellationToken)
    {
        await using var context = await _factory.CreateDbContextAsync(cancellationToken);

        var existed = (from c in context.Messages.AsEnumerable()
            join m in message on new { c.OwnerId, c.Id } equals new { m.OwnerId, m.Id }
            select new { c.OwnerId, c.Id })
                .ToArray();

        var toAdd = message.Where(f => !existed.Any(a => a.Id == f.Id && a.OwnerId == f.OwnerId))
            .Select(a => new VkEntity
            {
                Id = a.Id,
                OwnerId = a.OwnerId,
                Date = a.Date,
                Content = new VkMessageSearch
                {
                    Text = a.Text
                }
            });

        await context.AddRangeAsync(toAdd, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }
}
