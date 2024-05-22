using System.ComponentModel.DataAnnotations;

namespace CommandService;

public class Command
{
    [Key]
    [Required]
    public int Id { get; set; }
    public string? HowTo { get; set; }
    [Required]
    public required string CommandLine { get; set; }
    public int PlatformId { get; set; }
    public Platform Platform { get; set; }

}
