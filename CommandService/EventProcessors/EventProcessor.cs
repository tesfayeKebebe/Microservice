using System.Text.Json;
using CommandService.Dtos;
using CommandService.Enums;

namespace CommandService.EventProcessors;

public class EventProcessor(IServiceScopeFactory factory) : IEventProcessor
{
    private readonly IServiceScopeFactory _factory = factory;

    public async Task ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);
        switch (eventType)
        {
            case EventType.PlatformPublished:
               await AddPlatform(message);
                break;
            default:
                break;
            
        }
    }
    private EventType DetermineEvent(string notificationMessage)
    {
        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
        switch (eventType.Event)
        {
            case "Platform_Publish":
                Console.WriteLine("-->Platform published event detected");
                return EventType.PlatformPublished;
                    break;
            default:
                Console.WriteLine("-->Could not determined event type");
                return EventType.Undermined;
                break;
            
        }
    }

    private async Task AddPlatform(string message)
    {
        using var scope = _factory.CreateScope();
        try
        {
            var repo = scope.ServiceProvider.GetRequiredService<ICommandRepo>();
            var data = JsonSerializer.Deserialize<PlatformPublishedDto>(message);
            Platform plat = data;
            if (!await repo.ExternalPlatformExists(plat.ExternalId))
            {
                repo.CreatePlatform(plat);
                await repo.SaveChangesAsync();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine("");
            throw;
        }
    }
}