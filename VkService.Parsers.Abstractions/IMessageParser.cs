using VkService.Models;

namespace VkService.Parsers.Abstractions;

public interface IMessageParser
{
    IAsyncEnumerable<IEnumerable<RepostMessage>> Parse(CancellationToken cancellationToken);
}
