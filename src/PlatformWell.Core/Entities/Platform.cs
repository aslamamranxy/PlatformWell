using PlatformWell.Core.Models.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlatformWell.Core.Entities;

[Table("Platform")]
public class Platform : AuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public string? UniqueName { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime? LastUpdate { get; set; }
    public ICollection<Well>? Wells { get; set; }
}
