using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CommandService;

public class Platform
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required]
    public int ExternalId { get; set; }
    [Required]
    public required string Name { get; set; }
    public ICollection<Command> Commands { get; set; } = new List<Command>();
}
