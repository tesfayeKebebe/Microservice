namespace CommandService.Dtos;

public class PlatformPublishedDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public  string Event { get; set; }

    public static implicit operator Platform(PlatformPublishedDto publishedDto)
    {
        return new Platform
        {
          Id = publishedDto.Id,
          Name = publishedDto.Name,
          ExternalId = publishedDto.Id,
        };
    }
}