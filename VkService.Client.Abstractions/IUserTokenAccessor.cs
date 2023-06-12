namespace VkService.Client.Abstractions;

public interface IUserTokenAccessor
{
    ValueTask<string> GetTokenAsync();
}
