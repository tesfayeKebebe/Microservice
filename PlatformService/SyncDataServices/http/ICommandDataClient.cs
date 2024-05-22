namespace PlatformService.SyncDataServices.http;

public interface ICommandDataClient
{
Task SendPlatformToCommand(PlatformCreateDto platform);
}
