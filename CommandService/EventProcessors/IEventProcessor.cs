namespace CommandService.EventProcessors;

public interface IEventProcessor
{
    Task ProcessEvent(string message);
}