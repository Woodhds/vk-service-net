using VkService.Models;

namespace VkService.Client.Abstractions;

public interface IVkGroupService
{
    Task<VkResponse<IReadOnlyCollection<VkGroup>>> GetGroups(int count, int offset);
    Task LeaveGroup(int groupId);
    Task JoinGroup(int groupId);
}
