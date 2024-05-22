using System.ComponentModel.DataAnnotations;

namespace CommandService.Dtos;

public class CommandCreateDto
{
    public string? HowTo { get; set; }
    [Required]
    public required string CommandLine { get; set; }

    public static implicit operator Command(CommandCreateDto createDto)
    {
        return new Command
        {
            HowTo = createDto.HowTo,
            CommandLine = createDto.CommandLine
        };
    }
}