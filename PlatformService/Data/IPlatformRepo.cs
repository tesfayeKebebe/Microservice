using PlatformService.Models;

namespace PlatformService.Data;

public interface IPlatformRepo
{
    Task<bool>  SaveChangesAsync();
    Task<IEnumerable<Platform>> GetAll();
   Task<PlatformReadDto> Create(PlatformCreateDto platform);
    Task<Platform> GetById(int Id);
}
