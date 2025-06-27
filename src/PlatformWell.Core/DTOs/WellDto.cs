using PlatformWell.Core.Models.Core;

namespace PlatformWell.Core.DTOs;

public class WellDto : AuditableEntity
{
    public int Id { get; set; }
    public int PlatformId { get; set; }
    public string? UniqueName { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
}