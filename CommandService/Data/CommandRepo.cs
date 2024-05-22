using Microsoft.EntityFrameworkCore;

namespace CommandService.Data;

public class CommandRepo: ICommandRepo
{
    private readonly ApplicationDbContext _context;
    public CommandRepo(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }

    public async Task<IEnumerable<Platform>> GetAllPlatform()
    {
        return await _context.Platforms.ToListAsync();
    }

    public async Task<bool> PlatformExists(int platformId)
    {
       return await _context.Platforms.AnyAsync(x=>x.Id==platformId) ;
    }

    public async Task<bool> ExternalPlatformExists(int externalPlatformId)
    {
        return await _context.Platforms.AnyAsync(x=>x.ExternalId==externalPlatformId) ;
    }

    public void CreatePlatform(Platform platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }
        _context.Platforms.Add(platform);
        // await SaveChangesAsync();
    }

    public async Task<IEnumerable<Command>> GetCommandsPlatform(int platformId)
    {
      return  await _context.Commands
            .Where(x => x.PlatformId == platformId)
            .OrderBy(x => x.Platform.Name).ToListAsync();
    }

    public async Task<Command> GetCommand(int platformId, int commandId)
    {
       return await _context.Commands
           .FirstOrDefaultAsync(x=> x.PlatformId== platformId && x.Id ==commandId) ;
    }

    public void CreateCommand(int platformId, Command command)
    {
        if (command == null)
        {
            throw new ArgumentNullException(nameof(command));
        }

        command.PlatformId = platformId;
        _context.Commands.Add(command);
    }
}