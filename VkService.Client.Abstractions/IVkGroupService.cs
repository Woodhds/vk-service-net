using VkService.Models;

namespace VkService.Client.Abstractions;

public interface IVkGroupService
{
    Task<VkResponse<IReadOnlyCollection<VkGroup>>> GetGroups(int count, int offset);
    Task<VkResponse<IReadOnlyCollection<VkGroup>>> GetById(IEnumerable<int> ids, CancellationToken ct);
    Task LeaveGroup(int groupId);
    Task JoinGroup(int groupId);
}
