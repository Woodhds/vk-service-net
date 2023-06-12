using VkService.Models;

namespace VkService.Client.Abstractions;

public interface IVkWallService
{
    Task<VkResponse<IReadOnlyCollection<VkMessage>>> WallSearch(string id, int skip, int take, string? search = null);
    Task<VkResponse<IReadOnlyCollection<VkMessage>>> GetById(IEnumerable<RepostMessage>? vkRepostViewModels);
    Task<SimpleVkResponse<VkRepostMessage>> Repost(RepostMessage model);
}
