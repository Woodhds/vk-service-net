using VkService.Models;

namespace VkService.Client.Abstractions;

public interface IVkLikeService
{
    Task<SimpleVkResponse<VkResponseLikeModel>> Like(RepostMessage model);
}
