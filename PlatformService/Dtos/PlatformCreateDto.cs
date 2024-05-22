using System.ComponentModel.DataAnnotations;
using PlatformService.Models;

namespace PlatformService;

public class PlatformCreateDto
{

    [Required]
    public required string Name { get; set; }
    [Required]
    public required string Publisher { get; set; }
    [Required]
    public required string Costs { get; set; }
        public static implicit operator Platform(PlatformCreateDto platform)
    {
        return new Platform
        {
            Name = platform.Name,
            Costs = platform.Costs,
            Publisher = platform.Publisher
        };
    }
}
