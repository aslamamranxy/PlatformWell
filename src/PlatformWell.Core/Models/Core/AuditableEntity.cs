using PlatformWell.Core.Interfaces.Core;

namespace PlatformWell.Core.Models.Core;

public abstract class AuditableEntity : IAuditableEntity
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime? DeletedAt { get; set; }
}

public static class AuditableEntityExtensions
{
    public static IQueryable<T> IsNotDeleted<T>(this IQueryable<T> query) where T : AuditableEntity
            => query.Where(x => x.IsDeleted == false);
}
