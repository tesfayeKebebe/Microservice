namespace CommandService.Dtos;

public class CommandReadDto
{
    public int Id { get; set; }
    public string? HowTo { get; set; }
    public required string CommandLine { get; set; }
    public int PlatformId { get; set; }

    public static implicit operator CommandReadDto(Command command)
    {
        return new CommandReadDto
        {
                 CommandLine = command.CommandLine,
                 PlatformId = command.PlatformId,
                 Id = command.Id,
                 HowTo = command.HowTo
        };
    }
}