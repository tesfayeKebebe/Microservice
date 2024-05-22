using CommandService.Data;
using CommandService.EventProcessors;

namespace CommandService.Extensions;

public static class ServicesExtensions
{
 public static IServiceCollection AddServices(this IServiceCollection services)
 {
     services.AddScoped<ICommandRepo, CommandRepo>();
     services.AddSingleton<IEventProcessor, EventProcessor>();
    return services;
 }
}
