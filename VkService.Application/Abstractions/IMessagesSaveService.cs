
namespace VkService.Application.Abstractions;

public interface IMessagesSaveService
{
    Task Save(IEnumerable<Models.VkMessage> message, CancellationToken cancellationToken);
}
