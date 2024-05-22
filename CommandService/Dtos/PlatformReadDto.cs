using System.ComponentModel.DataAnnotations;

namespace CommandService.Dtos;

public class PlatformReadDto
{
    public int Id { get; set; }
    public required string Name { get; set; }

    public static implicit operator PlatformReadDto(Platform platform)
    {
        return new PlatformReadDto
        {
            Name = platform.Name,
            Id = platform.Id
        };
    }
}