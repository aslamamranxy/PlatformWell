using PlatformWell.Core.Models.Core;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlatformWell.Core.Entities;

[Table("Well")]
public class Well : AuditableEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    public int PlatformId { get; set; }
    [ForeignKey(nameof(PlatformId))]
    public Platform? Platform { get; set; }
    public string? UniqueName { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public DateTime? LastUpdate { get; set; }
}
