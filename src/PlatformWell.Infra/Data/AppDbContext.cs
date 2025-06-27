using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using PlatformWell.Core.Interfaces.Core;
using PlatformWell.Core.Entities;

namespace PlatformWell.Infra.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Platform> Platforms { get; set; }
    public DbSet<Well> Wells { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Platform>();

        modelBuilder.Entity<Well>()
            .HasOne(w => w.Platform)
            .WithMany(p => p.Wells)
            .HasForeignKey(w => w.PlatformId)
            .OnDelete(DeleteBehavior.Cascade);
    }

    public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
    {
        SetAuditable();

        var result = await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        return result;
    }

    private void SetAuditable()
    {
        var now = DateTime.UtcNow;

        foreach(EntityEntry entry in ChangeTracker.Entries())
        {
            if(entry.Entity is IAuditableEntity entity)
            {
                switch(entry.State)
                {
                    case EntityState.Added:
                        if (entity.CreatedAt == default) entity.CreatedAt = now;
                        break;
                    case EntityState.Modified:
                        if (entity.IsDeleted) entity.DeletedAt = now;
                        else entity.UpdatedAt = now;
                        break;
                    case EntityState.Unchanged:
                        break;
                }
            }
        }
    }
}
