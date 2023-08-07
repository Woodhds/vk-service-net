namespace VkService.Auth.Models;

public record IdentityUser
{
    public string FullName { get; set; }
    public string AccessToken { get; set; }
}