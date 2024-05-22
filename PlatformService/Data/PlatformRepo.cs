using Microsoft.EntityFrameworkCore;
using PlatformService.AsyncDataServices;
using PlatformService.Dtos;
using PlatformService.Models;
using PlatformService.SyncDataServices.http;

namespace PlatformService.Data;

public class PlatformRepo : IPlatformRepo
{
    private readonly AppDbContext _context;
    private readonly ICommandDataClient _commandDataClient;
    private readonly IMessageBusClient _messageBusClient;

    public PlatformRepo(AppDbContext context, 
        ICommandDataClient commandDataClient, 
        IMessageBusClient messageBusClient)
    {
        _context = context;
        _commandDataClient = commandDataClient;
        _messageBusClient = messageBusClient;
    }
    public async Task<PlatformReadDto> Create(PlatformCreateDto platform)
    {
        if (platform == null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        try
        {
            _context.Platforms.Add(platform);
            await SaveChangesAsync();
            var lastData =await _context.Platforms.OrderBy(x=>x.Id).LastOrDefaultAsync();
            //sync data messaging
            await  _commandDataClient.SendPlatformToCommand(platform);
            //async data messaging
            PlatformReadDto readDto = platform;
            readDto.Id = lastData.Id;
            PlatformPublishedDto publishedDto = readDto;
            publishedDto.Event = "Platform_Publish";
             _messageBusClient.PublishNewPlatform(publishedDto);
            
            return  readDto;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    
    }

    public async Task<IEnumerable<Platform>> GetAll()
    {
        return await _context.Platforms.ToListAsync();
    }

    public async Task<Platform> GetById(int Id)
    {
        return await _context.Platforms.FirstOrDefaultAsync(x => x.Id == Id);
    }

    public async Task<bool> SaveChangesAsync()
    {
        return (await _context.SaveChangesAsync() >= 0);
    }
}
