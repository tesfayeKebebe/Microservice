
using PlatformService.AsyncDataServices;
using PlatformService.SyncDataServices.http;

namespace PlatformService.Data;

public static class ServiceCollection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IPlatformRepo, PlatformRepo>();
        services.AddHttpClient<ICommandDataClient, CommandDataClient>();
        services.AddSingleton<IMessageBusClient, MessageBusClient>();
        return services;
    }
}
