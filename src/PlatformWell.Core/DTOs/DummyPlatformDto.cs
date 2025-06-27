namespace PlatformWell.Core.DTOs;

public class DummyPlatformDto
{
    public int Id { get; set; }
    public string? UniqueName { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime? LastUpdate  { get; set; }
    public List<DummyWellDto> Well { get; set; }
}