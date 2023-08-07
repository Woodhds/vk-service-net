using Microsoft.AspNetCore.Authentication.OAuth;

namespace VkService.Auth;

public class VkAuthOptions : OAuthOptions
{
    public VkAuthOptions()
    {
        AuthorizationEndpoint = "https://oauth.vk.com/authorize";
        UserInformationEndpoint = "https://api.vk.com/method/users.get";
        TokenEndpoint = "https://oauth.vk.com/access_token";
    }
    
    public string? Version { get; set; }
}