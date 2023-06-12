using VkService.Models;

namespace VkService.Client.Abstractions;

public interface IVkUserService
{
    Task<VkResponse<IReadOnlyCollection<VkUserResponse>>> SearchUserAsync(string search);
    Task<SimpleVkResponse<IReadOnlyCollection<VkUserResponse>>> GetUserInfo(string id);
}
