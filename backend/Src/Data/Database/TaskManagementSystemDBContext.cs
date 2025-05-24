using Data.Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace Data.Database;

public class TaskManagementSystemDBContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserTask> UserTasks { get; set; }

    public TaskManagementSystemDBContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("public");
    }

    protected static void _UpdateCreatedAtFieldIfExists(EntityEntry entry, DateTime DateTimeNow)
    {
        var entityType = entry.Entity.GetType();
        var hasProperty = entityType.GetProperties().Any(prop => prop.Name == "CreatedAt");
        if (hasProperty)
            entry.Property("CreatedAt").CurrentValue = DateTimeNow;
    }

    protected static void _UpdateUpdatedAtFieldIfExists(EntityEntry entry, DateTime DateTimeNow)
    {
        var entityType = entry.Entity.GetType();
        var hasProperty = entityType.GetProperties().Any(prop => prop.Name == "UpdatedAt");
        if (hasProperty)
            entry.Property("UpdatedAt").CurrentValue = DateTimeNow;
    }

    public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
    {
        DateTime DateTimeNow = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    {
                        _UpdateCreatedAtFieldIfExists(entry, DateTimeNow);
                        _UpdateUpdatedAtFieldIfExists(entry, DateTimeNow);
                        break;
                    }
                case EntityState.Modified:
                    {
                        _UpdateUpdatedAtFieldIfExists(entry, DateTimeNow);
                        break;
                    }
            }
        }
        return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }
}
