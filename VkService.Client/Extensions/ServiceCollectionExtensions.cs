using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VkService.Client.Abstractions;
using VkService.Client.Infrastructure.Serialization;
using VkService.Client.Options;
using VkService.Client.Services;
using VkService.Infrastructure.Serialization;

namespace VkService.Client.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddVkClients(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHttpClient<IVkClient, VkClient>()
            .AddHttpMessageHandler<VkClientHttpHandler>();

        services.Configure<VkontakteOptions>(configuration.GetSection("VkOptions"));

        services.AddSingleton<IVkWallService, VkWallService>();
        services.AddSingleton<IVkGroupService, VkGroupService>();
        services.AddSingleton<IVkUserService, VkUserService>();
        services.AddTransient<VkClientHttpHandler>();
        services.AddSingleton<IUserTokenAccessor, ConstantTokenAccessor>();
        services.AddSingleton<IJsonSerializer, JsonSerializer>();
        services.AddSingleton<IJsonSerializerOptionsProvider, JsonSerializerOptionsProvider>();
    }
}
