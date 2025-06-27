using PlatformWell.Core.Models.Core;

namespace PlatformWell.Core.DTOs;

public class PlatformDto : AuditableEntity
{
    public int Id { get; set; }
    public string? UniqueName { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public List<WellDto> Well { get; set; }
}
