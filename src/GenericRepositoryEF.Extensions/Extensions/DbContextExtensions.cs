using GenericRepositoryEF.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

namespace GenericRepositoryEF.Extensions.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="DbContext"/>.
    /// </summary>
    public static class DbContextExtensions
    {
        /// <summary>
        /// Applies global query filters for soft delete and multi-tenancy.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <returns>The model builder for method chaining.</returns>
        public static ModelBuilder ApplyGlobalFilters(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Apply soft delete filter
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    var parameter = System.Linq.Expressions.Expression.Parameter(entityType.ClrType, "e");
                    var property = System.Linq.Expressions.Expression.PropertyOrField(parameter, nameof(ISoftDelete.IsDeleted));
                    var falseConstant = System.Linq.Expressions.Expression.Constant(false);
                    var condition = System.Linq.Expressions.Expression.Equal(property, falseConstant);
                    var lambda = System.Linq.Expressions.Expression.Lambda(condition, parameter);

                    modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
                }
            }

            return modelBuilder;
        }

        /// <summary>
        /// Configures audit properties for entities.
        /// </summary>
        /// <param name="modelBuilder">The model builder.</param>
        /// <returns>The model builder for method chaining.</returns>
        public static ModelBuilder ConfigureAuditProperties(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Configure audit properties
                if (typeof(IAuditableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IAuditableEntity.CreatedAt))
                        .IsRequired();

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IAuditableEntity.CreatedBy))
                        .IsRequired()
                        .HasMaxLength(256);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IAuditableEntity.LastModifiedAt))
                        .IsRequired(false);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IAuditableEntity.LastModifiedBy))
                        .IsRequired(false)
                        .HasMaxLength(256);
                }
                
                // Configure soft delete properties
                if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(ISoftDelete.IsDeleted))
                        .IsRequired()
                        .HasDefaultValue(false);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(ISoftDelete.DeletedAt))
                        .IsRequired(false);

                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(ISoftDelete.DeletedBy))
                        .IsRequired(false)
                        .HasMaxLength(256);
                }
            }

            return modelBuilder;
        }
        
        /// <summary>
        /// Updates audit properties for entities before saving changes.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="userId">The user identifier.</param>
        public static void UpdateAuditProperties(this DbContext dbContext, string userId)
        {
            var entries = dbContext.ChangeTracker
                .Entries()
                .Where(e => e.Entity is IAuditableEntity && (
                    e.State == EntityState.Added || 
                    e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.Entity is IAuditableEntity auditableEntity)
                {
                    var now = DateTime.UtcNow;
                    
                    if (entityEntry.State == EntityState.Added)
                    {
                        auditableEntity.CreatedAt = now;
                        auditableEntity.CreatedBy = userId;
                    }
                    else
                    {
                        auditableEntity.LastModifiedAt = now;
                        auditableEntity.LastModifiedBy = userId;
                        
                        // Don't modify CreatedAt and CreatedBy
                        entityEntry.Property(nameof(IAuditableEntity.CreatedAt)).IsModified = false;
                        entityEntry.Property(nameof(IAuditableEntity.CreatedBy)).IsModified = false;
                    }
                }
            }
            
            // Handle soft delete
            var softDeleteEntries = dbContext.ChangeTracker
                .Entries()
                .Where(e => e.Entity is ISoftDelete && e.State == EntityState.Deleted);

            foreach (var entityEntry in softDeleteEntries)
            {
                entityEntry.State = EntityState.Modified;
                
                if (entityEntry.Entity is ISoftDelete softDelete)
                {
                    softDelete.IsDeleted = true;
                    softDelete.DeletedAt = DateTime.UtcNow;
                    softDelete.DeletedBy = userId;
                }
            }
        }
    }
}