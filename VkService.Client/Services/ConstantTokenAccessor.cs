using Microsoft.Extensions.Options;
using VkService.Client.Abstractions;
using VkService.Client.Options;

namespace VkService.Client.Services;

public sealed class ConstantTokenAccessor : IUserTokenAccessor
{
    private readonly IOptionsMonitor<VkontakteOptions> _options;

    public ConstantTokenAccessor(IOptionsMonitor<VkontakteOptions> options)
    {
        _options = options;
    }

    public ValueTask<string> GetTokenAsync()
    {
        return new ValueTask<string>(_options.CurrentValue.Token);
    }
}
