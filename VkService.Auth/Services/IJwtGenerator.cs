using VkService.Auth.Models;

namespace VkService.Auth.Services;

public interface IJwtGenerator
{
    public IdentityUser? Generate();
}
