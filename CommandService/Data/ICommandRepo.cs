namespace CommandService;

public interface ICommandRepo
{
    //Platform 
    Task<bool> SaveChangesAsync();
    Task<IEnumerable<Platform>> GetAllPlatform();
    Task<bool> PlatformExists(int platformId);
    Task<bool> ExternalPlatformExists(int externalPlatformId);
    void CreatePlatform(Platform platform);

    //Commands 
    Task<IEnumerable<Command>> GetCommandsPlatform(int platformId);
    Task<Command> GetCommand(int platformId, int commandId);
    void CreateCommand(int platformId, Command command);
}
