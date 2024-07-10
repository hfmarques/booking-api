using Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Data.Interceptors;

internal sealed class UpdateDatabaseEntityInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = new CancellationToken())
    {
        if (eventData.Context is null) return base.SavingChangesAsync(eventData, result, cancellationToken);

        var utcNow = DateTime.UtcNow;
        var entities = eventData.Context.ChangeTracker.Entries<DatabaseEntity>().ToList();

        foreach (var entity in entities)
        {
            if (entity.State is EntityState.Added)
            {
                entity.Property(nameof(DatabaseEntity.CreatedAt)).CurrentValue = utcNow;
            }
            else if (entity.State is EntityState.Modified)
            {
                entity.Property(nameof(DatabaseEntity.UpdatedAt)).CurrentValue = utcNow;
            }
            else if (entity.State is EntityState.Deleted)
            {
                entity.Property(nameof(DatabaseEntity.IsDeleted)).CurrentValue = true;
                entity.Property(nameof(DatabaseEntity.DeletedAt)).CurrentValue = DateTime.UtcNow;
                entity.State = EntityState.Modified;
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}