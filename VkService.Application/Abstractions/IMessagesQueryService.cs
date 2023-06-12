using VkService.Models;

namespace VkService.Application.Abstractions;

public interface IMessagesQueryService
{
    Task<IReadOnlyCollection<VkMessageModel>> GetMessages(string search);
}
