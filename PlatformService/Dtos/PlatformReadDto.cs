using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatformService.Models;

namespace PlatformService;

public class PlatformReadDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Publisher { get; set; }
    public required string Costs { get; set; }
    public static implicit operator PlatformReadDto(Platform platform)
    {
        return new PlatformReadDto
        {
            Id = platform.Id,
            Name = platform.Name,
            Costs = platform.Costs,
            Publisher = platform.Publisher
        };
    }
    public static implicit operator PlatformReadDto(PlatformCreateDto platform)
    {
        return new PlatformReadDto
        {
            Name = platform.Name,
            Costs = platform.Costs,
            Publisher = platform.Publisher
        };
    }

   
}
