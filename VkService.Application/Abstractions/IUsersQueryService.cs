using VkService.Models;

namespace VkService.Application.Abstractions;

public interface IUsersQueryService
{
    Task<IEnumerable<int>> GetAll(CancellationToken cancellationToken);
    Task<IEnumerable<VkUserModel>> GetFullUsers();
    Task Add(int id, string name, string avatar);
    Task Delete(int id);
}
