namespace PlatformService.Dtos;

public class PlatformPublishedDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public  string Event { get; set; }

    public static implicit operator PlatformPublishedDto(PlatformReadDto readDto)
    {
        return new PlatformPublishedDto
        {
          Id = readDto.Id,
          Name = readDto.Name
        };
    }
}